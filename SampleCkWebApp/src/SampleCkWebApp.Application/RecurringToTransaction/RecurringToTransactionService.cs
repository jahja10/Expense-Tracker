using ErrorOr;
using SampleCkWebApp.Application.RecurringTransactions.Interfaces.Infrastructure;
using SampleCkWebApp.Application.Transactions.Interfaces.Infrastructure;
using SampleCkWebApp.Domain.Entities;
using SampleCkWebApp.Domain.Enums;

namespace SampleCkWebApp.Application.RecurringToTransaction;

public class RecurringToTransactionService : IRecurringToTransactionService
{
    private readonly IRecurringTransactionRepository _recurringTransactionRepository;
    private readonly ITransactionRepository _transactionRepository;

    public RecurringToTransactionService(
        IRecurringTransactionRepository recurringTransactionRepository,
        ITransactionRepository transactionRepository)
    {
        _recurringTransactionRepository = recurringTransactionRepository;
        _transactionRepository = transactionRepository;
    }

    public async Task<ErrorOr<Success>> ProcessRecurringTransactionsAsync(
        int userId,
        CancellationToken cancellationToken)
    {
        var recurringResult = await _recurringTransactionRepository
            .GetRecurringTransactionsAsync(userId, cancellationToken);

        if (recurringResult.IsError)
            return recurringResult.Errors;

        var activeRecurring = recurringResult.Value
            .Where(x => x.IsActive)
            .ToList();

        var today = DateOnly.FromDateTime(DateTime.Today);

        foreach (var recurring in activeRecurring)
        {
            var nextDueDate = CalculateNextDueDate(recurring);

            while (nextDueDate <= today)
            {
                var transaction = new Transaction
                {
                    Price = recurring.Amount,
                    TransactionDate = nextDueDate,
                    TransactionType = TransactionType.Expense,
                    Description = recurring.Name,
                    Location = null,
                    UserId = recurring.UserId,
                    CategoryId = recurring.CategoryId,
                    PaymentMethodId = recurring.PaymentMethodId
                };

                var createTransactionResult = await _transactionRepository
                    .CreateTransactionAsync(transaction, cancellationToken);

                if (createTransactionResult.IsError)
                    return createTransactionResult.Errors;

                recurring.LastGeneratedDate = nextDueDate;

                var updateRecurringResult = await _recurringTransactionRepository
                    .UpdateRecurringTransactionAsync(recurring, cancellationToken);

                if (updateRecurringResult.IsError)
                    return updateRecurringResult.Errors;

                nextDueDate = CalculateNextDueDate(recurring);
            }
        }

        return Result.Success;
    }

    private static DateOnly CalculateNextDueDate(RecurringTransaction recurring)
{
    if (recurring.LastGeneratedDate is null)
        return recurring.StartDate;

    return recurring.FrequencyOfTransaction switch
    {
        FrequencyOfTransaction.Monthly => recurring.LastGeneratedDate.Value.AddMonths(1),
        FrequencyOfTransaction.Yearly => recurring.LastGeneratedDate.Value.AddYears(1),
        _ => recurring.StartDate
    };
}
}