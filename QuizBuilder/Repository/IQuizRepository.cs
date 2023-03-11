namespace QuizBuilder.Repository;

using Models.Data;
using Models.DTO;

public interface IQuizRepository
{
    Task PageAsync(PagingDTO<List<QuizList>> pagingDto, ApplicationUser currentUser, bool onlyTakenQuizzes);
    Task<Quiz> GetQuizByIdAsync(string id);
    Task<Quiz> GetQuizQuestionsByIdAsync(string id);
    Task<List<Answer>> GetAnswersToQuestionsAsync(List<int> questionIds);
    Task<List<UserAnsweredQuestion>> GetUserAnsweredQuestionsAsync(int submittedQuizId);
    Task<UserQuiz> GetSubmittedQuizAsync(string id, string currentUserId);
    Task DeleteAsync(Quiz quiz);
    Task AddUserQuiz(UserQuiz userQuiz);
    Task AddAsync(Quiz quiz, ApplicationUser currentUser);
    Task UpdateAsync(Quiz quiz);
    Task<List<UserQuiz>> GetSubmittedQuizzesAsync(string id);
    Task<List<UserAnsweredQuestion>> GetUserAnsweredQuestionsAsync(List<int> userQuizIds);
}