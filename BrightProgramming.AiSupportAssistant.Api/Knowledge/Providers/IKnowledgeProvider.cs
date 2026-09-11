using BrightProgramming.AiSupportAssistant.Api.Knowledge.Models;

namespace BrightProgramming.AiSupportAssistant.Api.Knowledge.Providers;

public interface IKnowledgeProvider
{
    string Name { get; }

    Task<KnowledgeContext> GetKnowledgeAsync(CancellationToken cancellationToken);
}