namespace SampleCkWebApp.Transactions;

public class GetTransactionsResponse
{
    public List<TransactionResponse> Transactions { get; set; } = new();

    public int TotalCount { get; set; }
}
