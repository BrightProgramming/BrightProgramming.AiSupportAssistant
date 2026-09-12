using BrightProgramming.AiSupportAssistant.Api.Ai.Answer.Factory;
using BrightProgramming.AiSupportAssistant.Api.Ai.Answer.Providers;
using BrightProgramming.AiSupportAssistant.Api.Knowledge.Models;

namespace BrightProgramming.AiSupportAssistant.Api.Ai.Answer.Processor;

public class AiAnswerProcessor : IAiAnswerProcessor
{
    private readonly IAiAnswerProvider _aiAnswerProvider;

    public AiAnswerProcessor(
        IAiAnswerProviderFactory aiAnswerProviderFactory)
    {
        _aiAnswerProvider = aiAnswerProviderFactory.GetProvider();
    }

    public Task<string> GetAnswerAsync(
        string question,
        KnowledgeContext knowledge,
        CancellationToken cancellationToken)
    {
        return _aiAnswerProvider.GetAnswerAsync(
            question,
            knowledge,
            cancellationToken);
    }
}