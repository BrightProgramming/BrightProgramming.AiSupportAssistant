using BrightProgramming.AiSupportAssistant.Api.Configuration;

namespace BrightProgramming.AiSupportAssistant.Api.StartupConfiguration;

public static class OptionsConfiguration
{
    public static IServiceCollection AddOptions(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddOptions<AiOptions>()
            .Bind(configuration.GetSection("Ai"))
            .Validate(
                options => !string.IsNullOrWhiteSpace(options.Provider),
                "AI provider must be configured.");

        services
            .AddOptions<KnowledgeOptions>()
            .Bind(configuration.GetSection("Knowledge"))
            .Validate(
                options => !string.IsNullOrWhiteSpace(options.Provider),
                "Knowledge provider must be configured.");

        services
            .AddOptions<OpenAiOptions>()
            .Bind(configuration.GetSection("OpenAI"))
            .Validate(
                options => !string.IsNullOrWhiteSpace(options.ApiKey),
                "OpenAI API key must be configured.")
            .Validate(
                options => !string.IsNullOrWhiteSpace(options.Model),
                "OpenAI model must be configured.");

        return services;
    }
}