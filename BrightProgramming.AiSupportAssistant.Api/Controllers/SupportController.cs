using BrightProgramming.AiSupportAssistant.Api.Ai;
using BrightProgramming.AiSupportAssistant.Api.Models.Api;
using BrightProgramming.AiSupportAssistant.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace BrightProgramming.AiSupportAssistant.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SupportController : ControllerBase
{
    private readonly IAiService _aiService;

    public SupportController(IAiService aiService)
    {
        _aiService = aiService;
    }

    [HttpPost()]
    public async Task<ActionResult<AskResponse>> AskAsync(
        AskRequest request)
    {
        var answer = await _aiService.GetAnswerAsync(request.Question);

        return Ok(new AskResponse
        {
            Question = request.Question,
            Answer = answer
        });
    }
}