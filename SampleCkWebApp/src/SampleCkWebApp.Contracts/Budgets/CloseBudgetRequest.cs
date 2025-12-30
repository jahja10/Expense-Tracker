namespace SampleCkWebApp.Contracts.Budgets;
public sealed record CloseBudgetRequest(
    DateOnly EndDate
);
