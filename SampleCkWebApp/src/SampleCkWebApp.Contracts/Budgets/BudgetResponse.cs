namespace SampleCkWebApp.Contracts.Budgets;

public sealed record BudgetResponse(
    int Id,
    decimal Amount,
    decimal? Savings,
    DateOnly StartDate,
    DateOnly? EndDate,
    int UserId
);
