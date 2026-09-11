using BrightProgramming.AiSupportAssistant.Api.Ai.Answer.Factory;
using BrightProgramming.AiSupportAssistant.Api.Ai.Answer.Processor;
using BrightProgramming.AiSupportAssistant.Api.Ai.Answer.Providers;
using BrightProgramming.AiSupportAssistant.Api.Ai.Answer.Providers.OpenAi;
using BrightProgramming.AiSupportAssistant.Api.Ai.Matcher.Factory;
using BrightProgramming.AiSupportAssistant.Api.Ai.Matcher.Processor;
using BrightProgramming.AiSupportAssistant.Api.Ai.Matcher.Providers;
using BrightProgramming.AiSupportAssistant.Api.Ai.Matcher.Providers.OpenAi;
using BrightProgramming.AiSupportAssistant.Api.Configuration;
using Microsoft.Extensions.Options;
using OpenAI.Chat;

namespace BrightProgramming.AiSupportAssistant.Api.StartupConfiguration;

public static class AiConfiguration
{
    public static IServiceCollection AddAi(
        this IServiceCollection services)
    {
        services.AddKeyedSingleton<ChatClient>(
            "AiAnswer",
            (serviceProvider, _) =>
            {
                var options = serviceProvider
                    .GetRequiredService<IOptions<AiAnswerOpenAiOptions>>()
                    .Value;

                return new ChatClient(
                    options.Model,
                    options.ApiKey);
            });

        services.AddKeyedSingleton<ChatClient>(
            "AiKnowledgeMatcher",
            (serviceProvider, _) =>
            {
                var options = serviceProvider
                    .GetRequiredService<IOptions<AiKnowledgeMatcherOpenAiOptions>>()
                    .Value;

                return new ChatClient(
                    options.Model,
                    options.ApiKey);
            });

        services.AddSingleton<IAiAnswerProvider, OpenAiAnswerProvider>();
        services.AddSingleton<IAiAnswerProviderFactory, AiAnswerProviderFactory>();
        services.AddSingleton<IAiAnswerProcessor, AiAnswerProcessor>();

        services.AddSingleton<IAiKnowledgeMatcherProvider, OpenAiKnowledgeMatcherProvider>();
        services.AddSingleton<IAiKnowledgeMatcherFactory, AiKnowledgeMatcherFactory>();
        services.AddSingleton<IAiKnowledgeProcessor, AiKnowledgeProcessor>();

        return services;
    }
}