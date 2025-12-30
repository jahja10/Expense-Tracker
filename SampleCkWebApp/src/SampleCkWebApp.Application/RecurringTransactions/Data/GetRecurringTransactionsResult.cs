using SampleCkWebApp.Domain.Entities;

namespace SampleCkWebApp.Application.RecurringTransactions.Data;

public class GetRecurringTransactionsResult
{
    

    public List<RecurringTransaction> RecurringTransactions { get; set; } = new ();

}