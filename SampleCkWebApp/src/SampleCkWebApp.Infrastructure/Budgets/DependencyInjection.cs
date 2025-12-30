using System.Transactions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SampleCkWebApp.Application.Budgets.Interfaces.Infrastructure;
using SampleCkWebApp.Infrastructure.Common;

namespace SampleCkWebApp.Infrastructure.Budgets;

public static class DependencyInjection
{
    public static IServiceCollection AddBudgetsInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var dbOptions = configuration.GetDatabaseOptions();
        services.TryAddDatabaseOptions(dbOptions);

        services.TryAddScoped<IBudgetRepository, BudgetRepository>();

        return services;
    }
}
