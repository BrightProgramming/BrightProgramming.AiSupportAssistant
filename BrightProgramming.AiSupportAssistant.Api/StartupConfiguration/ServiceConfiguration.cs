using BrightProgramming.AiSupportAssistant.Api.Services;

public static class ServiceConfiguration
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services)
    {
        services.AddScoped<ISupportService, SupportService>();

        return services;
    }
}