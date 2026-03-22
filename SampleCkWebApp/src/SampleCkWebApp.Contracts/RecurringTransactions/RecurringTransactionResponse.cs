namespace SampleCkWebApp.RecurringTransactions;

public class RecurringTransactionResponse
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string FrequencyOfTransaction { get; set; } = string.Empty;

    public int UserId { get; set; }

    public int CategoryId { get; set; }

    public int PaymentMethodId { get; set; }

    public decimal Amount { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly? LastGeneratedDate { get; set; }

    public bool IsActive { get; set; }
}
