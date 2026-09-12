using BrightProgramming.AiSupportAssistant.Api.Knowledge.Models;

namespace BrightProgramming.AiSupportAssistant.Api.Ai.Answer.Providers;

public interface IAiAnswerProvider
{
    string Name { get; }

    Task<string> GetAnswerAsync(
        string question,
        KnowledgeContext knowledge,
        CancellationToken cancellationToken);
}