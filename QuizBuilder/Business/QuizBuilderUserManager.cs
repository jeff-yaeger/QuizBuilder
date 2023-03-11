namespace QuizBuilder.Business;

using Microsoft.AspNetCore.Identity;
using Models.Data;

public class QuizBuilderUserManager : IQuizBuilderUserManager
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly UserManager<ApplicationUser> _userManager;

    public QuizBuilderUserManager(
        IHttpContextAccessor contextAccessor,
        UserManager<ApplicationUser> userManager
    )
    {
        _contextAccessor = contextAccessor;
        _userManager = userManager;
    }

    public async Task<ApplicationUser> GetUserAsync()
    {
        if (_contextAccessor.HttpContext == null)
        {
            return null;
        }

        return await _userManager.GetUserAsync(_contextAccessor.HttpContext.User);
    }
}