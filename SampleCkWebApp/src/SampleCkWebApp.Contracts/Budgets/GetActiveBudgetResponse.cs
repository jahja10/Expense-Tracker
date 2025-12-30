using SampleCkWebApp.Contracts.Budgets;

namespace SampleCkWebApp.Contracts.Budgets;
public sealed record GetActiveBudgetResponse(
    BudgetResponse? Budget
);
