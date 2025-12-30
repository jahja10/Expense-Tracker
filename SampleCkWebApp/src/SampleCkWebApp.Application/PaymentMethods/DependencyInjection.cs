using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SampleCkWebApp.Application.PaymentMethods.Interfaces.Application;
using SampleCkWebApp.Application.Users.Interfaces.Application;

namespace SampleCkWebApp.Application.PaymentMethods;

public static class DependencyInjection
{
    public static IServiceCollection AddPaymentMethodsApplication(this IServiceCollection services)
    {
        services.TryAddScoped<IPaymentMethodService, PaymentMethodService>();
        return services;
    }
}