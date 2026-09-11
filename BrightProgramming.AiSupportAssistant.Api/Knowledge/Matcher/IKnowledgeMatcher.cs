using BrightProgramming.AiSupportAssistant.Api.Knowledge.Models;

namespace BrightProgramming.AiSupportAssistant.Api.Knowledge;

public interface IKnowledgeMatcher
{
    KnowledgeContext Match(
        string question,
        KnowledgeContext knowledge);
}