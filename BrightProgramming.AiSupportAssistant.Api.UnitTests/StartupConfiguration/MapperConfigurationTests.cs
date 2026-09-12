using BrightProgramming.AiSupportAssistant.Api.Mappers;
using BrightProgramming.AiSupportAssistant.Api.StartupConfiguration;
using Microsoft.Extensions.DependencyInjection;

namespace BrightProgramming.AiSupportAssistant.Api.UnitTests.StartupConfiguration;

public class MapperConfigurationTests
{
    [Fact]
    public void AddMappers_RegistersSupportApiMapperAsSingleton()
    {
        var services = new ServiceCollection();

        services.AddMappers();

        var descriptor = Assert.Single(services);
        Assert.Equal(typeof(ISupportApiMapper), descriptor.ServiceType);
        Assert.Equal(typeof(SupportApiMapper), descriptor.ImplementationType);
        Assert.Equal(ServiceLifetime.Singleton, descriptor.Lifetime);
    }
}
