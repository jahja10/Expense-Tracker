namespace SampleCkWebApp.RecurringTransactions;

public class GetRecurringTransactionsResponse
{
    public List<RecurringTransactionResponse> RecurringTransactions { get; set; } = new();

    public int TotalCount { get; set; }
}
