using ErrorOr;
using SampleCkWebApp.Application.Categories.Interfaces.Infrastructure;
using SampleCkWebApp.Application.PaymentMethods.Interfaces.Infrastructure;
using SampleCkWebApp.Application.RecurringTransactions.Data;
using SampleCkWebApp.Application.RecurringTransactions.Interfaces.Application;
using SampleCkWebApp.Application.RecurringTransactions.Interfaces.Infrastructure;
using SampleCkWebApp.Application.Users.Interfaces.Infrastructure;
using SampleCkWebApp.Domain.Entities;
using SampleCkWebApp.Domain.Enums;
using SampleCkWebApp.Domain.Errors;

namespace SampleCkWebApp.Application.RecurringTransactions;

public class RecurringTransactionService : IRecurringTransactionService
{
    private readonly IRecurringTransactionRepository _recurringTransactionRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IPaymentMethodRepository _paymentMethodRepository;

    public RecurringTransactionService(
        IRecurringTransactionRepository recurringTransactionRepository,
        IUserRepository userRepository,
        ICategoryRepository categoryRepository,
        IPaymentMethodRepository paymentMethodRepository)
    {
        _recurringTransactionRepository = recurringTransactionRepository;
        _userRepository = userRepository;
        _categoryRepository = categoryRepository;
        _paymentMethodRepository = paymentMethodRepository;
    }

    public async Task<ErrorOr<GetRecurringTransactionsResult>> GetRecurringTransactionsAsync( int userId,  CancellationToken cancellationToken)
    {
        var recurringTransactionsResult =
            await _recurringTransactionRepository.GetRecurringTransactionsAsync(userId, cancellationToken);

        if (recurringTransactionsResult.IsError)
        {
            return recurringTransactionsResult.Errors;
        }

        return new GetRecurringTransactionsResult
        {
            RecurringTransactions = recurringTransactionsResult.Value
        };
    }

    public async Task<ErrorOr<RecurringTransaction>> GetRecurringTransactionByIdAsync( int id, int userId, CancellationToken cancellationToken)
    {
        var recurringTransactionResult =
            await _recurringTransactionRepository.GetRecurringTransactionByIdAsync(id, userId, cancellationToken);

        if (recurringTransactionResult.IsError)
        {
            return recurringTransactionResult.Errors;
        }

        return recurringTransactionResult.Value;
    }

    public async Task<ErrorOr<RecurringTransaction>> CreateRecurringTransactionAsync(string name,FrequencyOfTransaction frequencyOfTransaction,int userId,
        int categoryId,int paymentMethodId,decimal amount, DateOnly startDate, CancellationToken cancellationToken)
    {
        var userResult = await _userRepository.GetUserByIdAsync(userId, cancellationToken);
        if (userResult.IsError)
        {
            return UserErrors.NotFound;
        }

        var categoryResult =
            await _categoryRepository.GetCategoryByIdAsync(categoryId, userId, cancellationToken);
        if (categoryResult.IsError)
        {
            return CategoryErrors.NotFound;
        }

        var paymentMethodResult =
            await _paymentMethodRepository.GetPaymentMethodByIdAsync(paymentMethodId, userId, cancellationToken);
        if (paymentMethodResult.IsError)
        {
            return PaymentMethodErrors.NotFound;
        }

        var validationResult =
            RecurringTransactionValidator.ValidateCreateRecurringTransactionRequest(
                name,
                frequencyOfTransaction,
                startDate,
                userId,
                categoryId,
                paymentMethodId,
                amount);

        if (validationResult.IsError)
        {
            return validationResult.Errors;
        }

        var nameCheck =
            await _recurringTransactionRepository.GetRecurringTransactionByNameAsync(name, userId, cancellationToken);

        if (!nameCheck.IsError)
        {
            return RecurringTransactionErrors.DuplicateName;
        }

        var recurringTransaction = new RecurringTransaction
        {
            Name = name,
            FrequencyOfTransaction = frequencyOfTransaction,
            StartDate = startDate,
            LastGeneratedDate = null,
            IsActive = true,
            UserId = userId,
            CategoryId = categoryId,
            PaymentMethodId = paymentMethodId,
            Amount = amount
        };

        var createRecurringTransaction =
            await _recurringTransactionRepository.CreateRecurringTransactionAsync(recurringTransaction, cancellationToken);

        return createRecurringTransaction;
    }

    public async Task<ErrorOr<RecurringTransaction>> UpdateRecurringTransactionAsync(int id,int userId,string? name,FrequencyOfTransaction? frequencyOfTransaction,
    int? categoryId, int? paymentMethodId, decimal? amount, DateOnly? startDate, CancellationToken cancellationToken)
    {
        var existing =
            await _recurringTransactionRepository.GetRecurringTransactionByIdAsync(id, userId, cancellationToken);

        if (existing.IsError)
        {
            return existing.Errors;
        }

        if (!string.IsNullOrWhiteSpace(name))
        {
            var nameCheck =
                await _recurringTransactionRepository.GetRecurringTransactionByNameAsync(name, userId, cancellationToken);

            if (!nameCheck.IsError && nameCheck.Value.Id != existing.Value.Id)
            {
                return RecurringTransactionErrors.DuplicateName;
            }
        }

        var old = existing.Value;

        if (categoryId.HasValue)
        {
            var categoryResult =
                await _categoryRepository.GetCategoryByIdAsync(categoryId.Value, old.UserId, cancellationToken);

            if (categoryResult.IsError)
            {
                return CategoryErrors.NotFound;
            }
        }

        if (paymentMethodId.HasValue)
        {
            var paymentResult =
                await _paymentMethodRepository.GetPaymentMethodByIdAsync(paymentMethodId.Value, old.UserId, cancellationToken);

            if (paymentResult.IsError)
            {
                return PaymentMethodErrors.NotFound;
            }
        }

        var updated = new RecurringTransaction
        {
            Id = old.Id,
            UserId = old.UserId,
            Name = name ?? old.Name,
            FrequencyOfTransaction = frequencyOfTransaction ?? old.FrequencyOfTransaction,
            StartDate = startDate ?? old.StartDate,
            LastGeneratedDate = old.LastGeneratedDate,
            IsActive = old.IsActive,
            CategoryId = categoryId ?? old.CategoryId,
            PaymentMethodId = paymentMethodId ?? old.PaymentMethodId,
            Amount = amount ?? old.Amount
        };

        var validationResult =
            RecurringTransactionValidator.ValidateUpdateRecurringTransactionRequest(
                updated.Name,
                updated.FrequencyOfTransaction,
                updated.StartDate,
                updated.CategoryId,
                updated.PaymentMethodId,
                updated.Amount);

        if (validationResult.IsError)
        {
            return validationResult.Errors;
        }

        return await _recurringTransactionRepository.UpdateRecurringTransactionAsync(updated, cancellationToken);
    }

    public async Task<ErrorOr<bool>> DeleteRecurringTransactionAsync (int id,int userId, CancellationToken cancellationToken)

    {
        var deleteRecurringTransaction = await _recurringTransactionRepository.DeleteRecurringTransactionAsync(id, userId, cancellationToken);

        return deleteRecurringTransaction;
    }
}