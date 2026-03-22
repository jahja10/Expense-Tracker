using ErrorOr;

namespace SampleCkWebApp.Application.RecurringToTransaction;

public interface IRecurringToTransactionService
{
    Task<ErrorOr<Success>> ProcessRecurringTransactionsAsync(
        int userId,
        CancellationToken cancellationToken);
}