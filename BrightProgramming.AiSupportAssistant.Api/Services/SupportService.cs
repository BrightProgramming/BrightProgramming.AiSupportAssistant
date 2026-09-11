using BrightProgramming.AiSupportAssistant.Api.Ai;
using BrightProgramming.AiSupportAssistant.Api.Ai.Factory;
using BrightProgramming.AiSupportAssistant.Api.Models;

namespace BrightProgramming.AiSupportAssistant.Api.Services;

public class SupportService : ISupportService
{
    private readonly IAiProvider _aiProvider;

    public SupportService(IAiProviderFactory aiProviderFactory)
    {
        _aiProvider = aiProviderFactory.GetProvider();
    }

    public async Task<SupportResponse> GetAnswerAsync(SupportRequest request)
    {
        var answer = await _aiProvider.GetAnswerAsync(request.Question);

        return new SupportResponse
        {
            Question = request.Question,
            Answer = answer
        };
    }
}