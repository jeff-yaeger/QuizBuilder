namespace QuizBuilder.Business;

using ManagerResponses;
using Mappers;
using Models.Data;
using Models.DTO;
using Models.Enums;
using Repository;
using Validators;

public class QuizManager : IQuizManager
{
    private readonly IQuizBuilderUserManager _quizBuilderUserManager;
    private readonly IQuizRepository _quizRepository;

    public QuizManager(
        IQuizBuilderUserManager quizBuilderUserManager,
        IQuizRepository quizRepository)
    {
        _quizBuilderUserManager = quizBuilderUserManager;
        _quizRepository = quizRepository;
    }

    public async Task<IManagerResponse> GetAsync(int pageSize, int pageNumber, bool onlyTakenQuizzes)
    {
        var pagingDto = new PagingDTO<List<QuizList>>
        {
            PageSize = pageSize,
            PageNumber = pageNumber
        };

        var currentUser = await _quizBuilderUserManager.GetUserAsync();
        await _quizRepository.PageAsync(pagingDto, currentUser, onlyTakenQuizzes);
        return new OkResponse(pagingDto);
    }

    public async Task<IManagerResponse> CreateAsync(QuizDTO dto)
    {
        var currentUser = await _quizBuilderUserManager.GetUserAsync();
        var validator = new CreateQuizValidator(dto, currentUser);
        var response = validator.Validate();
        if (response != null)
        {
            return response;
        }

        var quiz = QuizMapper.Map(new Quiz(), dto);
        quiz.SetPublished();
        await _quizRepository.AddAsync(quiz, currentUser);

        return new CreatedResponse(quiz.Id);
    }

    public async Task<IManagerResponse> UpdateAsync(string id, QuizDTO dto)
    {
        if (id != dto.Id)
        {
            return new BadRequestResponse("Id's don't match");
        }

        var currentUser = await _quizBuilderUserManager.GetUserAsync();
        var quiz = await _quizRepository.GetQuizByIdAsync(id);
        var validator = new UpdateQuizValidator(quiz, dto, currentUser);
        var response = validator.Validate();
        if (response != null)
        {
            return response;
        }

        quiz = QuizMapper.Map(quiz, dto);
        quiz.SetPublished();

        await _quizRepository.UpdateAsync(quiz);
        return new OkResponse();
    }

    public async Task<IManagerResponse> DeleteAsync(string id)
    {
        var currentUser = await _quizBuilderUserManager.GetUserAsync();
        var quiz = await _quizRepository.GetQuizQuestionsByIdAsync(id);
        var validator = new DeleteQuizValidator(quiz, currentUser);
        var response = validator.Validate();
        if (response != null)
        {
            return response;
        }

        await _quizRepository.DeleteAsync(quiz);
        return new OkResponse();
    }

    public async Task<IManagerResponse> SubmitQuizAsync(UserQuizDTO dto)
    {
        var currentUser = await _quizBuilderUserManager.GetUserAsync();
        var submittedQuiz = await _quizRepository.GetSubmittedQuizAsync(dto.Id, currentUser.Id);
        var quiz = await _quizRepository.GetQuizQuestionsByIdAsync(dto.Id);
        var validator = new SubmitQuizValidator(quiz, submittedQuiz);
        var response = validator.Validate();
        if (response != null)
        {
            return response;
        }

        var userQuiz = new UserQuiz
        {
            User = currentUser,
            Quiz = quiz,
            UserAnsweredQuestions = new List<UserAnsweredQuestion>()
        };

        var questionsDict = await LoadQuestionAnswers(quiz);
        dto.RemoveDuplicateQuestions();
        decimal userPoints = 0;
        foreach (var questionDto in dto.Questions)
        {
            response = ScoreQuestion(questionDto, questionsDict, userQuiz, ref userPoints);
            if (response != null)
            {
                return response;
            }
        }

        userQuiz.Score = userPoints / quiz.TotalPossiblePoints * 100;
        await _quizRepository.AddUserQuiz(userQuiz);
        return new OkResponse($"{userQuiz.Score:0.00}");
    }

    public async Task<IManagerResponse> GetUserSolution(string quizId)
    {
        var currentUser = await _quizBuilderUserManager.GetUserAsync();
        var submittedQuiz = await _quizRepository.GetSubmittedQuizAsync(quizId, currentUser.Id);
        if (submittedQuiz == null)
        {
            return new BadRequestResponse("User hasn't taken this quiz");
        }

        var questions = await _quizRepository.GetUserAnsweredQuestionsAsync(submittedQuiz.Id);
        var userQuizDto = new UserQuizDTO
        {
            Id = quizId,
            Score = $"{submittedQuiz.Score:0.00}",
            Questions = new List<UserQuestionDTO>()
        };

        foreach (var question in questions)
        {
            userQuizDto.Questions.Add(new UserQuestionDTO
            {
                Id = question.Question.Id,
                Score = $"{question.Score:0.00}",
                Answers = question.SubmittedAnswers.Select(x => x.Id).ToList()
            });
        }

        return new OkResponse(userQuizDto);
    }

    public async Task<IManagerResponse> GetOtherUsersSolutions(string quizId)
    {
        var currentUser = await _quizBuilderUserManager.GetUserAsync();
        var quiz = await _quizRepository.GetQuizByIdAsync(quizId);
        var submittedQuizzes = await _quizRepository.GetSubmittedQuizzesAsync(quizId);
        var validator = new UserSolutionsValidator(quiz, submittedQuizzes, currentUser);
        var response = validator.Validate();
        if (response != null)
        {
            return response;
        }

        var userQuizIds = submittedQuizzes.Select(x => x.Id).ToList();
        var questionsLookup = (await _quizRepository.GetUserAnsweredQuestionsAsync(userQuizIds))
            .ToLookup(x => x.UserQuiz.Id);

        var result = new List<UserQuizDTO>(submittedQuizzes.Count);
        foreach (var submittedQuiz in submittedQuizzes)
        {
            BuildResponse(quizId, submittedQuiz, questionsLookup, result);
        }

        return new OkResponse(result);
    }

    private static void BuildResponse(string quizId, UserQuiz submittedQuiz, ILookup<int, UserAnsweredQuestion> questionsLookup, List<UserQuizDTO> result)
    {
        var userQuizDto = new UserQuizDTO
        {
            Id = quizId,
            Score = $"{submittedQuiz.Score:0.00}",
            Questions = new List<UserQuestionDTO>()
        };

        var questions = questionsLookup[submittedQuiz.Id];
        if (questions != null)
        {
            foreach (var question in questions)
            {
                userQuizDto.Questions.Add(new UserQuestionDTO
                {
                    Id = question.Question.Id,
                    Score = $"{question.Score:0.00}",
                    Answers = question.SubmittedAnswers.Select(x => x.Id).ToList()
                });
            }
        }

        result.Add(userQuizDto);
    }

    private static IManagerResponse ScoreQuestion(UserQuestionDTO questionDto, Dictionary<int, Question> questionsDict, UserQuiz userQuiz, ref decimal userPoints)
    {
        if (questionDto.Skipped() || !questionsDict.ContainsKey(questionDto.Id))
        {
            return null;
        }

        questionDto.RemoveDuplicateAnswers();
        var question = questionsDict[questionDto.Id];
        var userAnsweredQuestion = new UserAnsweredQuestion
        {
            Question = question,
            UserQuiz = userQuiz,
            SubmittedAnswers = new List<Answer>()
        };

        var scoreAnswers = ScoreAnswers(questionDto, question, userAnsweredQuestion);

        if (GetWeightedScore(questionDto, question, scoreAnswers, out var questionPoints, out var response))
        {
            return response;
        }

        userPoints += questionPoints;
        userAnsweredQuestion.Score = questionPoints * 100;

        userQuiz.UserAnsweredQuestions.Add(userAnsweredQuestion);
        return null;
    }

    private static bool GetWeightedScore(UserQuestionDTO questionDto, Question question, ScoreAnswersDTO scoreAnswers, out decimal questionPoints,
        out IManagerResponse scoreQuestion)
    {
        questionPoints = 0;
        if (question.TypeId == (int)QuestionTypeEnum.Single)
        {
            if (questionDto.Answers.Count > 1)
            {
                scoreQuestion = new BadRequestResponse("There can only be one answer for a single answer question.");
                return true;
            }

            if (scoreAnswers.Correct == 1)
            {
                questionPoints += 1;
            }
            else if (scoreAnswers.Incorrect > 0)
            {
                questionPoints -= 1;
            }
        }
        else
        {
            var correctAnswerWeight = (decimal)1 / question.CorrectAnswerCount;
            var incorrectAnswerWeight = (decimal)1 / question.IncorrectAnswerCount;

            if (scoreAnswers.Correct > 0)
            {
                questionPoints += correctAnswerWeight * scoreAnswers.Correct;
            }

            if (scoreAnswers.Incorrect > 0)
            {
                questionPoints -= incorrectAnswerWeight * scoreAnswers.Incorrect;
            }
        }

        scoreQuestion = null;
        return false;
    }

    private static ScoreAnswersDTO ScoreAnswers(UserQuestionDTO questionDto, Question question, UserAnsweredQuestion userAnsweredQuestion)
    {
        var result = new ScoreAnswersDTO();
        foreach (var answerId in questionDto.Answers)
        {
            if (question.IsCorrectAnswer(answerId))
            {
                result.Correct++;
            }
            else if (question.IsIncorrectAnswer(answerId))
            {
                result.Incorrect++;
            }

            var userAnswer = question.Answers.FirstOrDefault(x => x.Id == answerId);
            if (userAnswer != null)
            {
                userAnsweredQuestion.SubmittedAnswers.Add(userAnswer);
            }
        }

        return result;
    }

    private async Task<Dictionary<int, Question>> LoadQuestionAnswers(Quiz quiz)
    {
        var questionIds = quiz.Questions.Select(x => x.Id).ToList();
        var answers = (await _quizRepository.GetAnswersToQuestionsAsync(questionIds))
            .ToLookup(x => x.Question.Id);

        var questionsDict = new Dictionary<int, Question>();
        foreach (var question in quiz.Questions)
        {
            question.Answers = answers[question.Id].ToList();
            questionsDict.Add(question.Id, question);
        }

        return questionsDict;
    }

    private struct ScoreAnswersDTO
    {
        public int Correct { get; set; }
        public int Incorrect { get; set; }
    }
}