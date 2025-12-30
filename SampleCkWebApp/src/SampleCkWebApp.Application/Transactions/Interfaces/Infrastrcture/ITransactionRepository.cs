using ErrorOr;
using SampleCkWebApp.Domain.Entities;

namespace SampleCkWebApp.Application.Transactions.Interfaces.Infrastructure;


public interface ITransactionRepository
{
    

    Task <ErrorOr<List<Transaction>>> GetTransactionsAsync (CancellationToken cancellationToken);

    Task <ErrorOr<Transaction>> GetTransactionByIdAsync (int id, CancellationToken cancellationToken);

    Task <ErrorOr<Transaction>> CreateTransactionAsync (Transaction transaction, CancellationToken cancellationToken);

    Task <ErrorOr<Transaction>> UpdateTransactionAsync(Transaction transaction, CancellationToken cancellationToken);

    Task <ErrorOr<bool>> DeleteTransactionAsync (int id, CancellationToken cancellationToken);





}