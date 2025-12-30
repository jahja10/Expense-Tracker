using System.Transactions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SampleCkWebApp.Application.RecurringTransactions.Interfaces.Infrastructure;
using SampleCkWebApp.Infrastructure.Common;

namespace SampleCkWebApp.Infrastructure.RecurringTransactions;

public static class DependencyInjection
{
    public static IServiceCollection AddRecurringTransactionsInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var dbOptions = configuration.GetDatabaseOptions();
        services.TryAddDatabaseOptions(dbOptions);

        services.TryAddScoped<IRecurringTransactionRepository, RecurringTransactionRepository>();

        return services;
    }
}
