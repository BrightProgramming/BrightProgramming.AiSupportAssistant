using BrightProgramming.AiSupportAssistant.Api.Ai.Answer.Factory;
using BrightProgramming.AiSupportAssistant.Api.Ai.Answer.Processor;
using BrightProgramming.AiSupportAssistant.Api.Knowledge;
using BrightProgramming.AiSupportAssistant.Api.Knowledge.Factory;
using BrightProgramming.AiSupportAssistant.Api.Knowledge.Providers;
using BrightProgramming.AiSupportAssistant.Api.Models;

namespace BrightProgramming.AiSupportAssistant.Api.Services;

public class SupportService : ISupportService
{
    private readonly IAiAnswerProcessor _aiAnswerProcessor;
    private readonly IKnowledgeProvider _knowledgeProvider;
    private readonly IKnowledgeMatcher _knowledgeMatcher;

    public SupportService(
        IAiAnswerProcessor aiAnswerProcessor,
        IKnowledgeProviderFactory knowledgeProviderFactory,
        IKnowledgeMatcher knowledgeMatcher)
    {
        _aiAnswerProcessor = aiAnswerProcessor;
        _knowledgeProvider = knowledgeProviderFactory.GetProvider();
        _knowledgeMatcher = knowledgeMatcher;
    }

    public async Task<SupportResponse> GetAnswerAsync(
        SupportRequest request,
        CancellationToken cancellationToken)
    {
        var knowledge = await _knowledgeProvider.GetKnowledgeAsync(
            cancellationToken);

        var matchedKnowledge = await _knowledgeMatcher.MatchAsync(
            request.Question,
            knowledge,
            cancellationToken);

        var answer = await _aiAnswerProcessor.GetAnswerAsync(
            request.Question,
            matchedKnowledge,
            cancellationToken);

        return new SupportResponse
        {
            Question = request.Question,
            Answer = answer
        };
    }
}