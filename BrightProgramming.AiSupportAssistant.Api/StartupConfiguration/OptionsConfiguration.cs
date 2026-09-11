using BrightProgramming.AiSupportAssistant.Api.Configuration;

namespace BrightProgramming.AiSupportAssistant.Api.StartupConfiguration;

public static class OptionsConfiguration
{
    public static IServiceCollection AddOptions(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddOptions<AiAnswerOptions>()
            .Bind(configuration.GetSection("AiAnswer"))
            .Validate(
                options => !string.IsNullOrWhiteSpace(options.Provider),
                "AI provider must be configured.");

        services
            .AddOptions<KnowledgeSourceOptions>()
            .Bind(configuration.GetSection("KnowledgeSource"))
            .Validate(
                options => !string.IsNullOrWhiteSpace(options.Provider),
                "Knowledge source provider must be configured.");

        services
            .AddOptions<AiKnowledgeMatcherOptions>()
            .Bind(configuration.GetSection("AiKnowledgeMatcher"))
            .Validate(
                options => !string.IsNullOrWhiteSpace(options.Provider),
                "AI knowledge matcher provider must be configured.");

        services
            .AddOptions<AiAnswerOpenAiOptions>()
            .Bind(configuration.GetSection("AiAnswerOpenAI"))
            .Validate(
                options => !string.IsNullOrWhiteSpace(options.ApiKey),
                "AI Answer OpenAI API key must be configured.")
            .Validate(
                options => !string.IsNullOrWhiteSpace(options.Model),
                "AI Answer OpenAI model must be configured.");

        services
            .AddOptions<AiKnowledgeMatcherOpenAiOptions>()
            .Bind(configuration.GetSection("AiKnowledgeMatcherOpenAI"))
            .Validate(
                options => !string.IsNullOrWhiteSpace(options.ApiKey),
                "AI Knowledge Matcher OpenAI API key must be configured.")
            .Validate(
                options => !string.IsNullOrWhiteSpace(options.Model),
                "AI Knowledge Matcher OpenAI model must be configured.");

        return services;
    }
}