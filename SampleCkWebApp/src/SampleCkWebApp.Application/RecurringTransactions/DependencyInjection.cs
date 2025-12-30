using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SampleCkWebApp.Application.RecurringTransactions.Interfaces.Application;


namespace SampleCkWebApp.Application.RecurringTransactions;

public static class DependencyInjection
{
    public static IServiceCollection AddCategoriesApplication(this IServiceCollection services)
    {
        services.TryAddScoped<IRecurringTransactionService, RecurringTransactionService>();
        return services;
    }
}