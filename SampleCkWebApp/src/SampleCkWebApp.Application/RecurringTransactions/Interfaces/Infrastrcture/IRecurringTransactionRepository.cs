using ErrorOr;
using SampleCkWebApp.Domain.Entities;

namespace SampleCkWebApp.Application.RecurringTransactions.Interfaces.Infrastructure;


public interface IRecurringTransactionRepository
{
    

    Task<ErrorOr<List<RecurringTransaction>>> GetRecurringTransactionsAsync(int userId, CancellationToken cancellationToken);

    Task<ErrorOr<RecurringTransaction>> GetRecurringTransactionByIdAsync(int id, int userId, CancellationToken cancellationToken);

    Task<ErrorOr<RecurringTransaction>> CreateRecurringTransactionAsync
    (RecurringTransaction recurringTransaction, CancellationToken cancellationToken);

    Task<ErrorOr<RecurringTransaction>> UpdateRecurringTransactionAsync
    (RecurringTransaction recurringTransaction, CancellationToken cancellationToken);

    Task<ErrorOr<bool>> DeleteRecurringTransactionAsync(int id, int userId, CancellationToken cancellationToken);




}