using BrightProgramming.AiSupportAssistant.Api.Ai;
using BrightProgramming.AiSupportAssistant.Api.Models;

namespace BrightProgramming.AiSupportAssistant.Api.Services;

public class SupportService : ISupportService
{
    private readonly IAiService _aiService;

    public SupportService(IAiService aiService)
    {
        _aiService = aiService;
    }

    public async Task<SupportResponse> GetAnswerAsync(SupportRequest request)
    {
        var answer = await _aiService.GetAnswerAsync(request.Question);

        return new SupportResponse
        {
            Question = request.Question,
            Answer = answer
        };
    }
}