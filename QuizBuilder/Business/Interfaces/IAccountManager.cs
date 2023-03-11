namespace QuizBuilder.Business.Interfaces;

using ManagerResponses;
using Models.DTO;

public interface IAccountManager
{
    Task<IManagerResponse> LoginAsync(LoginModel model);
    Task<IManagerResponse> Register(RegisterModel model);
}