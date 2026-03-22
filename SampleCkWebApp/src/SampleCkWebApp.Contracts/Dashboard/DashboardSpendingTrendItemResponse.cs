namespace SampleCkWebApp.Contracts.Dashboard;

public sealed record DashboardSpendingTrendItemResponse(
    string Month,
    decimal Spent,
    decimal Budget
);