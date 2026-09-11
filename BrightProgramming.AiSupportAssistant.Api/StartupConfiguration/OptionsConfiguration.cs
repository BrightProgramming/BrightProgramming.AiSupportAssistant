namespace BrightProgramming.AiSupportAssistant.Api.StartupConfiguration;

public static class OptionsConfiguration
{
    public static IServiceCollection AddOptions(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<AiOptions>(
            configuration.GetSection("Ai"));

        return services;
    }
}