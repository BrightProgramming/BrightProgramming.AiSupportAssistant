using BrightProgramming.AiSupportAssistant.Api.Ai.Matcher.Providers;
using BrightProgramming.AiSupportAssistant.Api.Configuration;
using Microsoft.Extensions.Options;

namespace BrightProgramming.AiSupportAssistant.Api.Ai.Matcher.Factory;

public class AiKnowledgeMatcherFactory : IAiKnowledgeMatcherFactory
{
    private readonly IAiKnowledgeMatcherProvider _provider;

    public AiKnowledgeMatcherFactory(
        IEnumerable<IAiKnowledgeMatcherProvider> providers,
        IOptions<AiKnowledgeMatcherOptions> options)
    {
        var providerName = options.Value.Provider;

        _provider = providers.SingleOrDefault(x => string.Equals(
            x.Name,
            providerName,
            StringComparison.OrdinalIgnoreCase))
            ?? throw new InvalidOperationException(
                $"AI knowledge matcher provider '{providerName}' is not registered.");
    }

    public IAiKnowledgeMatcherProvider GetProvider()
    {
        return _provider;
    }
}