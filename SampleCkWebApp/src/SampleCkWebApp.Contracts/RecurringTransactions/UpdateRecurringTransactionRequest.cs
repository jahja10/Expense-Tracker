namespace SampleCkWebApp.RecurringTransactions;

public class UpdateRecurringTransactionRequest
{
    public string? Name { get; set; }

    public FrequencyOfTransactionContract? FrequencyOfTransaction { get; set; }

    public DateOnly? NextRunDate { get; set; }

    public int? CategoryId { get; set; }

    public int? PaymentMethodId { get; set; }
}
