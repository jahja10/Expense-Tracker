using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SampleCkWebApp.Application.Transactions.Interfaces.Application;


namespace SampleCkWebApp.Application.Transactions;

public static class DependencyInjection
{
    public static IServiceCollection AddCategoriesApplication(this IServiceCollection services)
    {
        services.TryAddScoped<ITransactionService, TransactionService>();
        return services;
    }
}