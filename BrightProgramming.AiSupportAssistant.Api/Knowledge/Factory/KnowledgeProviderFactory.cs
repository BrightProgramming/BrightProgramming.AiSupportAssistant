using BrightProgramming.AiSupportAssistant.Api.Configuration;
using BrightProgramming.AiSupportAssistant.Api.Knowledge.Providers;
using Microsoft.Extensions.Options;

namespace BrightProgramming.AiSupportAssistant.Api.Knowledge.Factory;

public class KnowledgeProviderFactory : IKnowledgeProviderFactory
{
    private readonly IKnowledgeProvider _provider;

    public KnowledgeProviderFactory(
        IEnumerable<IKnowledgeProvider> providers,
        IOptions<KnowledgeOptions> options)
    {
        var providerName = options.Value.Provider;

        _provider = providers.SingleOrDefault(x => string.Equals(
            x.Name,
            providerName,
            StringComparison.OrdinalIgnoreCase))
            ?? throw new InvalidOperationException(
                $"Knowledge provider '{providerName}' is not registered.");
    }

    public IKnowledgeProvider GetProvider()
    {
        return _provider;
    }
}
