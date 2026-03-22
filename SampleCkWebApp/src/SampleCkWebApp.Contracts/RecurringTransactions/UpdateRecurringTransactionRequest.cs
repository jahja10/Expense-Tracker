namespace SampleCkWebApp.RecurringTransactions;

public class UpdateRecurringTransactionRequest
{
    public string? Name { get; set; }

    public FrequencyOfTransactionContract? FrequencyOfTransaction { get; set; }

    public int? CategoryId { get; set; }

    public int? PaymentMethodId { get; set; }

    public decimal? Amount { get; set; }

    public DateOnly? StartDate { get; set; }

}
