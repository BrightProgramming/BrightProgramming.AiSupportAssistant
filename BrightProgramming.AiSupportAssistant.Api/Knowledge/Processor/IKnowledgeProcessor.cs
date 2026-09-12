using BrightProgramming.AiSupportAssistant.Api.Knowledge.Models;

namespace BrightProgramming.AiSupportAssistant.Api.Knowledge.Processor;

public interface IKnowledgeProcessor
{
    Task<KnowledgeContext> GetRelevantKnowledgeAsync(
        string question,
        CancellationToken cancellationToken);
}