using BrightProgramming.AiSupportAssistant.Api.Ai;
using BrightProgramming.AiSupportAssistant.Api.Ai.Factories;
using BrightProgramming.AiSupportAssistant.Api.Models;

namespace BrightProgramming.AiSupportAssistant.Api.Services;

public class SupportService : ISupportService
{
    private readonly IAiProvider _aiProvider;

    public SupportService(IAiProviderFactory aiProviderFactory)
    {
        _aiProvider = aiProviderFactory.GetProvider();
    }

    public async Task<SupportResponse> GetAnswerAsync(SupportRequest request, CancellationToken cancellationToken)
    {
        var answer = await _aiProvider.GetAnswerAsync(request.Question, cancellationToken);

        return new SupportResponse
        {
            Question = request.Question,
            Answer = answer
        };
    }
}