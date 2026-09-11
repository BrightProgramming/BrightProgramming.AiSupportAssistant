using BrightProgramming.AiSupportAssistant.Api.Knowledge.Providers;

namespace BrightProgramming.AiSupportAssistant.Api.Knowledge.Factory
{
    public interface IKnowledgeProviderFactory
    {
        IKnowledgeProvider GetProvider();
    }
}
