using BrightProgramming.AiSupportAssistant.Api.Knowledge.Models;

namespace BrightProgramming.AiSupportAssistant.Api.Ai.Answer.Processor;

public interface IAiAnswerProcessor
{
    Task<string> GetAnswerAsync(
        string question,
        KnowledgeContext knowledge,
        CancellationToken cancellationToken);
}