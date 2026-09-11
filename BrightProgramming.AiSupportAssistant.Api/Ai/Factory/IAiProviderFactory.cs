using BrightProgramming.AiSupportAssistant.Api.Ai.Providers;

namespace BrightProgramming.AiSupportAssistant.Api.Ai.Factory;

public interface IAiProviderFactory
{
    IAiProvider GetProvider();
}