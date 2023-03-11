namespace QuizBuilder.Controllers;

using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTO;

[ApiController]
[Route("[controller]")]
public class AccountController : BaseController
{
    private readonly IAccountManager _accountManager;

    public AccountController(IAccountManager accountManager)
    {
        _accountManager = accountManager;
    }

    [AllowAnonymous]
    [HttpPost("Register")]
    public async Task<IActionResult> RegisterAsync(RegisterModel model)
    {
        var response = await _accountManager.Register(model);
        return GetResponse(response);
    }

    [AllowAnonymous]
    [HttpPost("Login")]
    public async Task<IActionResult> LoginAsync(LoginModel model)
    {
        var response = await _accountManager.LoginAsync(model);
        return GetResponse(response);
    }
}