using BrightProgramming.AiSupportAssistant.Api.Ai.Matcher.Processor;
using BrightProgramming.AiSupportAssistant.Api.Knowledge.Factory;
using BrightProgramming.AiSupportAssistant.Api.Knowledge.Models;
using BrightProgramming.AiSupportAssistant.Api.Knowledge.Processor;
using BrightProgramming.AiSupportAssistant.Api.Knowledge.Providers;

namespace BrightProgramming.AiSupportAssistant.Api.Knowledge.Processor;

public class KnowledgeProcessor : IKnowledgeProcessor
{
    private readonly IKnowledgeProvider _knowledgeProvider;
    private readonly IAiKnowledgeProcessor _aiKnowledgeProcessor;

    public KnowledgeProcessor(
        IKnowledgeProviderFactory knowledgeProviderFactory,
        IAiKnowledgeProcessor aiKnowledgeProcessor)
    {
        _knowledgeProvider = knowledgeProviderFactory.GetProvider();
        _aiKnowledgeProcessor = aiKnowledgeProcessor;
    }

    public async Task<KnowledgeContext> GetRelevantKnowledgeAsync(
        string question,
        CancellationToken cancellationToken)
    {
        var knowledge = await _knowledgeProvider.GetKnowledgeAsync(
            cancellationToken);

        return await _aiKnowledgeProcessor.MatchAsync(
            question,
            knowledge,
            cancellationToken);
    }
}