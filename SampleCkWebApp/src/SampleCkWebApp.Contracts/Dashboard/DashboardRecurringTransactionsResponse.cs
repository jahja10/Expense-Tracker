namespace SampleCkWebApp.Contracts.Dashboard;

public sealed record DashboardRecurringTransactionsResponse
(
    string Name,
    decimal Amount,
    DateOnly NextDueDate

);