namespace QuizBuilder.Business.Validators;

using ManagerResponses;
using Models.Data;
using Models.DTO;
using Util;

public class CreateQuizValidator
{
    private readonly ApplicationUser _currentUser;
    private readonly QuizDTO _dto;

    public CreateQuizValidator(QuizDTO dto, ApplicationUser currentUser)
    {
        _dto = dto;
        _currentUser = currentUser;
    }

    public IManagerResponse Validate()
    {
        if (_currentUser == null)
        {
            return new BadRequestResponse("User is invalid");
        }

        if (string.IsNullOrWhiteSpace(_dto.Title))
        {
            return new BadRequestResponse("A title is required");
        }

        if (_dto.Questions.IsNullOrEmpty())
        {
            return null;
        }

        if (_dto.Questions.Count > 10)
        {
            return new BadRequestResponse("A quiz is not allowed to have more than 10 questions");
        }

        var errors = new List<string>();
        foreach (var question in _dto.Questions)
        {
            ValidateQuestion(question, errors);
        }

        if (errors.Count == 0)
        {
            return null;
        }

        var badRequest = new BadRequestResponse();
        errors.ForEach(error => badRequest.AddError(error));
        return badRequest;
    }

    private static void ValidateQuestion(QuestionDTO question, ICollection<string> errors)
    {
        if (string.IsNullOrWhiteSpace(question.Value))
        {
            errors.Add("Question is required to have a text value.");
        }

        if (question.Answers.IsNullOrEmpty())
        {
            errors.Add($"Question {question.Value} is required to have answers");
            return;
        }

        if (question.Answers.Any(x => string.IsNullOrWhiteSpace(x.Value)))
        {
            errors.Add("All Answers are required to have a text value.");
        }

        if (question.Answers.Count < 2)
        {
            errors.Add($"Question {question.Value} has too few answers");
        }

        if (question.Answers.Count > 5)
        {
            errors.Add($"Question {question.Value} has too many answers");
        }

        if (question.CorrectAnswerCount == 0)
        {
            errors.Add($"Question {question.Value} needs at least one correct answer");
        }
    }
}