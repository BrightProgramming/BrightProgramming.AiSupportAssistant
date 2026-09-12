using BrightProgramming.AiSupportAssistant.Api.Ai.Answer.Factory;
using BrightProgramming.AiSupportAssistant.Api.Ai.Answer.Processor;
using BrightProgramming.AiSupportAssistant.Api.Ai.Answer.Providers;
using BrightProgramming.AiSupportAssistant.Api.Ai.Answer.Providers.OpenAi;
using BrightProgramming.AiSupportAssistant.Api.Ai.Matcher.Factory;
using BrightProgramming.AiSupportAssistant.Api.Ai.Matcher.Processor;
using BrightProgramming.AiSupportAssistant.Api.Ai.Matcher.Providers;
using BrightProgramming.AiSupportAssistant.Api.Ai.Matcher.Providers.OpenAi;
using BrightProgramming.AiSupportAssistant.Api.Constants;
using BrightProgramming.AiSupportAssistant.Api.StartupConfiguration;
using Microsoft.Extensions.DependencyInjection;
using OpenAI.Chat;

namespace BrightProgramming.AiSupportAssistant.Api.UnitTests.StartupConfiguration;

public class AiConfigurationTests
{
    [Fact]
    public void AddAi_RegistersRequiredServicesWithExpectedLifetimes()
    {
        var services = new ServiceCollection();

        services.AddAi();

        AssertSingleton<IAiAnswerProvider, OpenAiAnswerProvider>(services);
        AssertSingleton<IAiAnswerProviderFactory, AiAnswerProviderFactory>(services);
        AssertSingleton<IAiAnswerProcessor, AiAnswerProcessor>(services);
        AssertSingleton<IAiKnowledgeMatcherProvider, OpenAiKnowledgeMatcherProvider>(services);
        AssertSingleton<IAiKnowledgeMatcherFactory, AiKnowledgeMatcherFactory>(services);
        AssertSingleton<IAiKnowledgeProcessor, AiKnowledgeProcessor>(services);
        AssertKeyedSingleton<ChatClient>(services, AiClient.Answer);
        AssertKeyedSingleton<ChatClient>(services, AiClient.KnowledgeMatcher);
    }

    private static void AssertSingleton<TService, TImplementation>(IServiceCollection services)
        where TImplementation : TService
    {
        var descriptor = Assert.Single(services, x =>
            x.ServiceType == typeof(TService) && x.ServiceKey is null);

        Assert.Equal(typeof(TImplementation), descriptor.ImplementationType);
        Assert.Equal(ServiceLifetime.Singleton, descriptor.Lifetime);
    }

    private static void AssertKeyedSingleton<TService>(IServiceCollection services, object key)
    {
        var descriptor = Assert.Single(services, x =>
            x.ServiceType == typeof(TService) && Equals(x.ServiceKey, key));

        Assert.NotNull(descriptor.KeyedImplementationFactory);
        Assert.Equal(ServiceLifetime.Singleton, descriptor.Lifetime);
    }
}
