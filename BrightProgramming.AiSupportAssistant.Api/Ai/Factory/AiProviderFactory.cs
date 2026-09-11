using BrightProgramming.AiSupportAssistant.Api.Ai.Providers;
using Microsoft.Extensions.Options;

namespace BrightProgramming.AiSupportAssistant.Api.Ai.Factory;

public class AiProviderFactory : IAiProviderFactory
{
    private readonly IAiProvider _provider;

    public AiProviderFactory(
        IEnumerable<IAiProvider> providers,
        IOptions<AiOptions> options)
    {
        var providerName = options.Value.Provider;

        _provider = providers.SingleOrDefault(x => string.Equals(
            x.Name,
            providerName,
            StringComparison.OrdinalIgnoreCase))
            ?? throw new InvalidOperationException(
                $"AI provider '{providerName}' is not registered.");
    }

    public IAiProvider GetProvider()
    {
        return _provider;
    }
}