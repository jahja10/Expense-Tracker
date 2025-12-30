using SampleCkWebApp.Domain.Entities;
using SampleCkWebApp.Application.RecurringTransactions.Data;
using SampleCkWebApp.RecurringTransactions;

namespace SampleCkWebApp.Application.RecurringTransactions.Mappings;

public static class RecurringTransactions
{
    public static GetRecurringTransactionsResponse ToResponse(this GetRecurringTransactionsResult result)
    {
        return new GetRecurringTransactionsResponse
        {
            RecurringTransactions = result.RecurringTransactions.Select(t => t.ToResponse()).ToList(),
            TotalCount = result.RecurringTransactions.Count
        };
    }

    public static RecurringTransactionResponse ToResponse(this RecurringTransaction recurringTransaction)
    {
        return new RecurringTransactionResponse
        {
            Id = recurringTransaction.Id,
            Name = recurringTransaction.Name,
            FrequencyOfTransaction = recurringTransaction.FrequencyOfTransaction.ToString(),
            NextRunDate = recurringTransaction.NextRunDate,
            UserId = recurringTransaction.UserId,
            CategoryId = recurringTransaction.CategoryId,
            PaymentMethodId = recurringTransaction.PaymentMethodId
        };
    }
}
