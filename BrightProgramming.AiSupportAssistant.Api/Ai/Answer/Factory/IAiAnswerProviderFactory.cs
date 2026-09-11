using BrightProgramming.AiSupportAssistant.Api.Ai.Answer.Providers;

namespace BrightProgramming.AiSupportAssistant.Api.Ai.Answer.Factory;

public interface IAiAnswerProviderFactory
{
    IAiAnswerProvider GetProvider();
}