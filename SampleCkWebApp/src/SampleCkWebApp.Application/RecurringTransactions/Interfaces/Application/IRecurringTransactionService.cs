using ErrorOr;
using SampleCkWebApp.Application.RecurringTransactions.Data;
using SampleCkWebApp.Domain.Entities;
using SampleCkWebApp.Domain.Enums;

namespace SampleCkWebApp.Application.RecurringTransactions.Interfaces.Application;



public interface IRecurringTransactionService
{
    

    Task<ErrorOr<GetRecurringTransactionsResult>> GetRecurringTransactionsAsync(int userId, CancellationToken cancellationToken);

    Task<ErrorOr<RecurringTransaction>> GetRecurringTransactionByIdAsync(int id, int userId, CancellationToken cancellationToken);

    Task<ErrorOr<RecurringTransaction>> CreateRecurringTransactionAsync(string name, FrequencyOfTransaction 
    frequencyOfTransaction, DateOnly? nextRunDate, int userId, int categoryId, int paymentMethodId, CancellationToken cancellationToken);

    Task<ErrorOr<RecurringTransaction>> UpdateRecurringTransactionAsync(int id, int userId, string? name, 
    FrequencyOfTransaction? frequencyOfTransaction, DateOnly? nextRunDate, int? categoryId, int? paymentMethodId,
    CancellationToken cancellationToken);

    Task<ErrorOr<bool>> DeleteRecurringTransactionAsync(int id, int userId, CancellationToken cancellationToken);







}