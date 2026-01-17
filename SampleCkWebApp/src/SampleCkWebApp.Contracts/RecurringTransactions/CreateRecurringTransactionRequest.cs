namespace SampleCkWebApp.RecurringTransactions;

public class CreateRecurringTransactionRequest
{
    public string Name { get; set; } = string.Empty;

    public FrequencyOfTransactionContract FrequencyOfTransaction { get; set; }

    public DateOnly? NextRunDate { get; set; }

    public int CategoryId { get; set; }

    public int PaymentMethodId { get; set; }
}
