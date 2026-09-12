using BrightProgramming.AiSupportAssistant.Api.Ai.Answer.Providers;
using BrightProgramming.AiSupportAssistant.Api.Configuration;
using Microsoft.Extensions.Options;

namespace BrightProgramming.AiSupportAssistant.Api.Ai.Answer.Factory;

public class AiAnswerProviderFactory : IAiAnswerProviderFactory
{
    private readonly IAiAnswerProvider _provider;

    public AiAnswerProviderFactory(
        IEnumerable<IAiAnswerProvider> providers,
        IOptions<AiAnswerOptions> options)
    {
        var providerName = options.Value.Provider;

        _provider = providers.SingleOrDefault(x => string.Equals(
            x.Name,
            providerName,
            StringComparison.OrdinalIgnoreCase))
            ?? throw new InvalidOperationException(
                $"AI provider '{providerName}' is not registered.");
    }

    public IAiAnswerProvider GetProvider()
    {
        return _provider;
    }
}