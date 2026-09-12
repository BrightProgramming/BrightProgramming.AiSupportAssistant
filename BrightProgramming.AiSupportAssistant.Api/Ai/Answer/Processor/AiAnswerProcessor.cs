using BrightProgramming.AiSupportAssistant.Api.Ai.Answer.Factory;
using BrightProgramming.AiSupportAssistant.Api.Knowledge.Models;

namespace BrightProgramming.AiSupportAssistant.Api.Ai.Answer.Processor;

public class AiAnswerProcessor : IAiAnswerProcessor
{
    private readonly IAiAnswerProviderFactory _aiAnswerProviderFactory;

    public AiAnswerProcessor(
        IAiAnswerProviderFactory aiAnswerProviderFactory)
    {
        _aiAnswerProviderFactory = aiAnswerProviderFactory;
    }

    public Task<string> GetAnswerAsync(
        string question,
        KnowledgeContext knowledge,
        CancellationToken cancellationToken)
    {
        var provider = _aiAnswerProviderFactory.GetProvider();

        return provider.GetAnswerAsync(
            question,
            knowledge,
            cancellationToken);
    }
}