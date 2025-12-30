using SampleCkWebApp.Contracts.Budgets;
using SampleCkWebApp.Domain.Entities;

namespace SampleCkWebApp.WebApi.Mappings;

public static class BudgetMappings
{
    public static BudgetResponse ToResponse(this Budget budget)
        => new(
            budget.Id,
            budget.Amount,
            budget.Savings,
            budget.StartDate,
            budget.EndDate,
            budget.UserId);
}
