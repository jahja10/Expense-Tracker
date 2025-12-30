namespace SampleCkWebApp.Transactions;

public class UpdateTransactionRequest
{
    public decimal? Price { get; set; }

    public DateOnly? TransactionDate { get; set; }

    public TransactionTypeContract? TransactionType { get; set; }

    public string? Description { get; set; }

    public string? Location { get; set; }

    public int? CategoryId { get; set; }

    public int? PaymentMethodId { get; set; }
}
