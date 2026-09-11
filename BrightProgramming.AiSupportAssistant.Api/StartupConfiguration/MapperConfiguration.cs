namespace BrightProgramming.AiSupportAssistant.Api.StartupConfiguration;

using BrightProgramming.AiSupportAssistant.Api.Mappers;

public static class MapperConfiguration
{
    public static IServiceCollection AddMappers(
        this IServiceCollection services)
    {
        services.AddSingleton<ISupportApiMapper, SupportApiMapper>();

        return services;
    }
}