using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SampleCkWebApp.Application.MessageHistory;
using SampleCkWebApp.Infrastructure.MessageHistory;
using SampleCkWebApp.Infrastructure.MessageHistory.Options;
using SampleCkWebApp.WebApi.Options;

namespace SampleCkWebApp.WebApi;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.TryAddMessageHistoryOptions(configuration.GetMessageHistoryOptions());

        return services
            .AddMessageHistoryApplication();
    }
    
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddMessageHistoryInfrastructure(configuration);
    }
}