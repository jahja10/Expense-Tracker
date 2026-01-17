using ErrorOr;
using SampleCkWebApp.Application.Transactions.Data;
using SampleCkWebApp.Domain.Entities;
using SampleCkWebApp.Domain.Enums;

namespace SampleCkWebApp.Application.Transactions.Interfaces.Application;


public interface ITransactionService
{
    

    Task <ErrorOr<GetTransactionsResult>> GetTransactionsAsync (int userId, CancellationToken cancellationToken);

    Task <ErrorOr<Transaction>> GetTransactionByIdAsync (int id, int userId, CancellationToken cancellationToken);

    Task <ErrorOr<Transaction>> CreateTransactionAsync (decimal? price, DateOnly? transactionDate,
    TransactionType transactionType, string? description, string? location, int userId, int categoryId, int paymentMethodId,
    CancellationToken cancellationToken);

    Task <ErrorOr<Transaction>> UpdateTransactionAsync (int id, int userId, decimal? price, DateOnly? transactionDate,
    TransactionType? transactionType, string? description, string? location, int? categoryId, int? paymentMethodId,
    CancellationToken cancellationToken);

    Task <ErrorOr<bool>> DeleteTransactionAsync (int id, int userId, CancellationToken cancellationToken);

   

    





}