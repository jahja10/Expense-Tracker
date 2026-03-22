namespace SampleCkWebApp.Contracts.Dashboard;


public sealed record DashboardTransactionsResponse
(
    decimal Price,
    DateOnly TransactionDate,
    string TransactionType,
    string? Description,
    string? Location,
    string CategoryName,
    string PaymentMethodName


);