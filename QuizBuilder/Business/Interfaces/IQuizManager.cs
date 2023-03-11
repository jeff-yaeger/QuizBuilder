namespace QuizBuilder.Business;

using ManagerResponses;
using Models.DTO;

public interface IQuizManager
{
    Task<IManagerResponse> GetAsync(int pageSize, int pageNumber, bool onlyTakenQuizzes);
    Task<IManagerResponse> CreateAsync(QuizDTO dto);
    Task<IManagerResponse> UpdateAsync(string id, QuizDTO dto);
    Task<IManagerResponse> DeleteAsync(string id);
    Task<IManagerResponse> SubmitQuizAsync(UserQuizDTO dto);
    Task<IManagerResponse> GetUserSolution(string quizId);
    Task<IManagerResponse> GetOtherUsersSolutions(string quizId);
}