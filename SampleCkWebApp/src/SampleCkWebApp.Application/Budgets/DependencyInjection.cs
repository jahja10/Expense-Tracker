using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SampleCkWebApp.Application.Budgets.Interfaces.Application;


namespace SampleCkWebApp.Application.Budgets;

public static class DependencyInjection
{
    public static IServiceCollection AddBudgetsApplication(this IServiceCollection services)
    {
        services.TryAddScoped<IBudgetService, BudgetService>();
        return services;
    }
}