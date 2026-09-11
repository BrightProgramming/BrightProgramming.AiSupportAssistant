using BrightProgramming.AiSupportAssistant.Api.Ai.Factory;
using BrightProgramming.AiSupportAssistant.Api.Ai.Providers;
using BrightProgramming.AiSupportAssistant.Api.Knowledge.Factory;
using BrightProgramming.AiSupportAssistant.Api.Knowledge.Providers;
using BrightProgramming.AiSupportAssistant.Api.Models;

namespace BrightProgramming.AiSupportAssistant.Api.Services;

public class SupportService : ISupportService
{
    private readonly IAiProvider _aiProvider;
    private readonly IKnowledgeProvider _knowledgeProvider;

    public SupportService(IAiProviderFactory aiProviderFactory,
        IKnowledgeProviderFactory knowledgeProviderFactory)
    {
        _aiProvider = aiProviderFactory.GetProvider();
        _knowledgeProvider = knowledgeProviderFactory.GetProvider();
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