using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SampleCkWebApp.Application.PaymentMethods.Interfaces.Infrastructure;
using SampleCkWebApp.Infrastructure.Common;

namespace SampleCkWebApp.Infrastructure.PaymentMethods;

public static class DependencyInjection
{
    public static IServiceCollection AddPaymentMethodsInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var dbOptions = configuration.GetDatabaseOptions();
        services.TryAddDatabaseOptions(dbOptions);

        services.TryAddScoped<IPaymentMethodRepository, PaymentMethodRepository>();

        return services;
    }
}
