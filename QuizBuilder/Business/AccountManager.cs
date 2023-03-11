namespace QuizBuilder.Business;

using Interfaces;
using ManagerResponses;
using Microsoft.AspNetCore.Identity;
using Models.Data;
using Models.DTO;
using Models.Enums;
using StringUtil = Util.StringUtil;

public class AccountManager : IAccountManager
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITokenManager _tokenManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public AccountManager(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ITokenManager tokenManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenManager = tokenManager;
    }

    public async Task<IManagerResponse> LoginAsync(LoginModel model)
    {
        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
        if (!result.Succeeded)
        {
            return new BadRequestResponse("User name or password was incorrect");
        }

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            return new NotFoundResponse("User not found");
        }

        var token = _tokenManager.CreateToken(user.Id);
        return new OkResponse(token);
    }

    public async Task<IManagerResponse> Register(RegisterModel model)
    {
        var user = new ApplicationUser
        {
            Email = model.Email,
            UserName = model.Email
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            return new BadRequestResponse(GetError(result));
        }

        return new OkResponse((int)ResponseEnum.Success);
    }

    private static string GetError(IdentityResult result)
    {
        return StringUtil.Join(result.Errors.Select(x => x.Description), " ");
    }
}