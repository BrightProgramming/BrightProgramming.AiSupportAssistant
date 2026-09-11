using BrightProgramming.AiSupportAssistant.Api.Ai;
using BrightProgramming.AiSupportAssistant.Api.Mappers;
using BrightProgramming.AiSupportAssistant.Api.Models.Api;
using BrightProgramming.AiSupportAssistant.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace BrightProgramming.AiSupportAssistant.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SupportController : ControllerBase
{
    private readonly ISupportService _supportService;
    private readonly ISupportApiMapper _mapper;

    public SupportController(ISupportService supportService, ISupportApiMapper mapper)
    {
        _supportService = supportService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult<AskResponse>> AskAsync(AskRequest request, CancellationToken cancellationToken)
    {
        var supportRequest = _mapper.ToSupportRequest(request);
        var supportResponse = await _supportService.GetAnswerAsync(supportRequest, cancellationToken);
        var mappedAnswer = _mapper.ToAskResponse(supportResponse);

        return Ok(mappedAnswer);
    }
}