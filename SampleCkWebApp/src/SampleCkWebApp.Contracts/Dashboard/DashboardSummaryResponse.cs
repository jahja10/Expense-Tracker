namespace SampleCkWebApp.Contracts.Dashboard;

public sealed record DashboardSummaryResponse(
    string UserName,
    decimal SpentThisMonth,
    decimal MonthlyBudget,
    DateOnly? BudgetEndDate,
    List<DashboardRecurringTransactionsResponse> RecurringTransactions,
    int TotalTransactionsThisMonth,
    decimal UpcomingPaymentsThisMonth,
    decimal AverageDailySpending

);
