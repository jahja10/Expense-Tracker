
namespace SampleCkWebApp.Contracts.Budgets;
public sealed record CreateBudgetRequest(
    decimal Amount,
    decimal? Savings,
    DateOnly? EndDate
);

