using ErrorOr;
using SampleCkWebApp.Application.RecurringTransactions.Data;
using SampleCkWebApp.Domain.Entities;
using SampleCkWebApp.Domain.Enums;

namespace SampleCkWebApp.Application.RecurringTransactions.Interfaces.Application;



public interface IRecurringTransactionService
{
    

    Task<ErrorOr<GetRecurringTransactionsResult>> GetRecurringTransactionsAsync(CancellationToken cancellationToken);

    Task<ErrorOr<RecurringTransaction>> GetRecurringTransactionByIdAsync(int id, CancellationToken cancellationToken);

    Task<ErrorOr<RecurringTransaction>> CreateRecurringTransactionAsync(string name, FrequencyOfTransaction 
    frequencyOfTransaction, DateOnly? nextRunDate, int userId, int categoryId, int paymentMethodId, CancellationToken cancellationToken);

    Task<ErrorOr<RecurringTransaction>> UpdateRecurringTransactionAsync(int id, string? name, 
    FrequencyOfTransaction? frequencyOfTransaction, DateOnly? nextRunDate, int? categoryId, int? paymentMethodId,
    CancellationToken cancellationToken);

    Task<ErrorOr<bool>> DeleteRecurringTransactionAsync(int id, CancellationToken cancellationToken);







}