namespace SampleCkWebApp.RecurringTransactions;

public class CreateRecurringTransactionRequest
{
    public string Name { get; set; } = string.Empty;

    public FrequencyOfTransactionContract FrequencyOfTransaction { get; set; }

    public int CategoryId { get; set; }

    public int PaymentMethodId { get; set; }

    public decimal Amount { get; set; }

    public DateOnly StartDate { get; set; }

}
