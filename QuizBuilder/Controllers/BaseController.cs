namespace QuizBuilder.Controllers;

using ManagerResponses;
using Microsoft.AspNetCore.Mvc;
using Models.DTO;
using StringUtil = Util.StringUtil;

public class BaseController : ControllerBase
{
    protected IActionResult GetResponse(IManagerResponse response)
    {
        return response.HasResponse()
            ? StatusCode(response.GetStatusCode(), response.GetResponse())
            : StatusCode(response.GetStatusCode());
    }
}