namespace QuizBuilder.Repository;

using Data;
using Microsoft.EntityFrameworkCore;
using Models.Data;
using Models.DTO;
using Util;

public class QuizRepository : IQuizRepository
{
    private readonly ApplicationDbContext _context;

    public QuizRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task PageAsync(PagingDTO<List<QuizList>> pagingDto, ApplicationUser currentUser, bool onlyTakenQuizzes)
    {
        var userQuizScores = from uq in _context.UserQuizzes
            where uq.UserId == currentUser.Id
            select new UserScore { Score = uq.Score, QuizId = uq.QuizId };

        var query = from q in _context.Quizzes
            join userQuizScore in userQuizScores on q.Id equals userQuizScore.QuizId into u_join
            from userQuizScore in u_join.DefaultIfEmpty()
            where q.Published
            let hasTaken = userQuizScore.Score != null
            orderby q.CreatedDate descending
            select new QuizList
            {
                Id = q.Id,
                Title = q.Title,
                QuestionCount = q.Questions.Count,
                HasTaken = hasTaken,
                Score = hasTaken ? $"{userQuizScore.Score:0.00}" : string.Empty
            };

        if (onlyTakenQuizzes)
        {
            query = query.Where(x => x.HasTaken);
        }

        pagingDto.TotalRecords = await query.CountAsync();
        pagingDto.SetLimits();
        pagingDto.Data = await query
            .Skip(pagingDto.PageSize * (pagingDto.PageNumber - 1))
            .Take(pagingDto.PageSize)
            .ToListAsync();
    }

    public async Task<Quiz> GetQuizByIdAsync(string id)
    {
        return await _context.Quizzes
            .Include(x => x.CreatedUser)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<Quiz> GetQuizQuestionsByIdAsync(string id)
    {
        return await _context.Quizzes
            .Include(x => x.CreatedUser)
            .Include(x => x.Questions)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Answer>> GetAnswersToQuestionsAsync(List<int> questionIds)
    {
        return await _context.Answers
            .Where(x => questionIds.Contains(x.Question.Id))
            .ToListAsync();
    }

    public async Task<List<UserAnsweredQuestion>> GetUserAnsweredQuestionsAsync(List<int> userQuizIds)
    {
        return await _context.UserAnsweredQuestion
            .Include(x => x.SubmittedAnswers)
            .Include(x => x.Question)
            .Include(x => x.UserQuiz)
            .Where(x => userQuizIds.Contains(x.UserQuiz.Id))
            .ToListAsync();
    }

    public async Task<List<UserAnsweredQuestion>> GetUserAnsweredQuestionsAsync(int submittedQuizId)
    {
        return await _context.UserAnsweredQuestion
            .Include(x => x.SubmittedAnswers)
            .Include(x => x.Question)
            .Where(x => x.UserQuiz.Id == submittedQuizId)
            .ToListAsync();
    }

    public async Task<UserQuiz> GetSubmittedQuizAsync(string id, string currentUserId)
    {
        return await _context.UserQuizzes
            .Where(x => x.QuizId == id && x.User.Id == currentUserId)
            .FirstOrDefaultAsync();
    }

    public async Task<List<UserQuiz>> GetSubmittedQuizzesAsync(string id)
    {
        return await _context.UserQuizzes
            .Where(x => x.QuizId == id)
            .ToListAsync();
    }

    public async Task AddUserQuiz(UserQuiz userQuiz)
    {
        _context.UserQuizzes.Add(userQuiz);
        await _context.SaveChangesAsync();
    }

    public async Task AddAsync(Quiz quiz, ApplicationUser currentUser)
    {
        quiz.CreatedUser = currentUser;
        quiz.CreatedDate = DateTime.UtcNow;
        var notAdded = true;
        while (notAdded)
        {
            try
            {
                quiz.Id = Guid.NewGuid().ToString();
                _context.Quizzes.Add(quiz);
                await _context.SaveChangesAsync();
                notAdded = false;
            }
            catch (Exception e)
            {
                //catch any conflict error
                notAdded = true;
            }
        }
    }

    public async Task UpdateAsync(Quiz quiz)
    {
        _context.Quizzes.Update(quiz);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Quiz quiz)
    {
        if (!quiz.Questions.IsNullOrEmpty())
        {
            var questionIds = quiz.Questions.Select(x => x.Id).ToList();
            var answers = await _context.Answers.Where(x => questionIds.Contains(x.Question.Id)).ToListAsync();
            _context.Answers.RemoveRange(answers);
            _context.Questions.RemoveRange(quiz.Questions);
        }

        _context.Quizzes.Remove(quiz);
        await _context.SaveChangesAsync();
    }

    private struct UserScore
    {
        public decimal? Score { get; set; }
        public string QuizId { get; set; }
    }
}