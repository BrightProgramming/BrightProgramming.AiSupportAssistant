using BrightProgramming.AiSupportAssistant.Api.Knowledge.Factory;
using BrightProgramming.AiSupportAssistant.Api.Knowledge.Processor;
using BrightProgramming.AiSupportAssistant.Api.Knowledge.Providers;
using BrightProgramming.AiSupportAssistant.Api.Knowledge.Providers.Markdown;
using BrightProgramming.AiSupportAssistant.Api.StartupConfiguration;
using Microsoft.Extensions.DependencyInjection;

namespace BrightProgramming.AiSupportAssistant.Api.UnitTests.StartupConfiguration;

public class KnowledgeConfigurationTests
{
    [Fact]
    public void AddKnowledge_RegistersRequiredServicesAsSingletons()
    {
        var services = new ServiceCollection();

        services.AddKnowledge();

        AssertSingleton<IKnowledgeProvider, MarkdownKnowledgeProvider>(services);
        AssertSingleton<IKnowledgeProviderFactory, KnowledgeProviderFactory>(services);
        AssertSingleton<IKnowledgeProcessor, KnowledgeProcessor>(services);
    }

    private static void AssertSingleton<TService, TImplementation>(IServiceCollection services)
        where TImplementation : TService
    {
        var descriptor = Assert.Single(services, x => x.ServiceType == typeof(TService));
        Assert.Equal(typeof(TImplementation), descriptor.ImplementationType);
        Assert.Equal(ServiceLifetime.Singleton, descriptor.Lifetime);
    }
}
