namespace QuizBuilder.Business;

using Models.Data;

public interface IQuizBuilderUserManager
{
    Task<ApplicationUser> GetUserAsync();
}