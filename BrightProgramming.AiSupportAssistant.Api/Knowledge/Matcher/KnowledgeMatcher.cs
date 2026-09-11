using BrightProgramming.AiSupportAssistant.Api.Knowledge.Models;

namespace BrightProgramming.AiSupportAssistant.Api.Knowledge;

public class KnowledgeMatcher : IKnowledgeMatcher
{
    public KnowledgeContext Match(string question, KnowledgeContext knowledge)
    {
        return knowledge;
    }
}