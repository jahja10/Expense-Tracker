

namespace SampleCkWebApp.Transactions;

public class TransactionResponse
{
    public int Id { get; set; }

    public decimal? Price { get; set; }

    public DateOnly? TransactionDate { get; set; }

    public string TransactionType { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? Location { get; set; }

    public int UserId { get; set; }

    public int CategoryId { get; set; }

    public int PaymentMethodId { get; set; }
}
