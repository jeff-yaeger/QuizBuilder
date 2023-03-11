namespace QuizBuilder.Controllers;

using Business;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTO;

[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("[controller]")]
public class QuizController : BaseController
{
    private readonly IQuizManager _quizManager;

    public QuizController(IQuizManager quizManager)
    {
        _quizManager = quizManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync(int pageSize, int pageNumber, bool onlyTakenQuizzes = false)
    {
        var response = await _quizManager.GetAsync(pageSize, pageNumber, onlyTakenQuizzes);
        return GetResponse(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(QuizDTO dto)
    {
        var response = await _quizManager.CreateAsync(dto);
        return GetResponse(response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(string id, QuizDTO dto)
    {
        var response = await _quizManager.UpdateAsync(id, dto);
        return GetResponse(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        var response = await _quizManager.DeleteAsync(id);
        return GetResponse(response);
    }

    [HttpPost("SubmitQuiz")]
    public async Task<IActionResult> SubmitQuizAsync(UserQuizDTO dto)
    {
        var response = await _quizManager.SubmitQuizAsync(dto);
        return GetResponse(response);
    }

    [HttpGet("Solution/{id}")]
    public async Task<IActionResult> GetUserSolution(string id)
    {
        var response = await _quizManager.GetUserSolution(id);
        return GetResponse(response);
    }

    [HttpGet("Solution/All/{id}")]
    public async Task<IActionResult> GetOtherUsersSolutions(string id)
    {
        var response = await _quizManager.GetOtherUsersSolutions(id);
        return GetResponse(response);
    }
}