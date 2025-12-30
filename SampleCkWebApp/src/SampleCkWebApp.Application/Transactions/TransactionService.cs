using ErrorOr;
using SampleCkWebApp.Application.Categories.Interfaces.Infrastructure;
using SampleCkWebApp.Application.PaymentMethods.Interfaces.Infrastructure;
using SampleCkWebApp.Application.Transactions.Data;
using SampleCkWebApp.Application.Transactions.Interfaces.Application;
using SampleCkWebApp.Application.Transactions.Interfaces.Infrastructure;
using SampleCkWebApp.Application.Users.Interfaces.Infrastructure;
using SampleCkWebApp.Domain.Entities;
using SampleCkWebApp.Domain.Enums;
using SampleCkWebApp.Domain.Errors;



namespace SampleCkWebApp.Application.Transactions;


public class TransactionService : ITransactionService
{
    
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUserRepository _userRepository;

    private readonly ICategoryRepository _categoryRepository;

    private readonly IPaymentMethodRepository _paymentMethodRepository;

    public TransactionService(ITransactionRepository transactionRepository, IUserRepository userRepository, 
    ICategoryRepository categoryRepository, IPaymentMethodRepository paymentMethodRepository)
    {
        
        _transactionRepository = transactionRepository;
        _userRepository = userRepository;
        _categoryRepository = categoryRepository;
        _paymentMethodRepository = paymentMethodRepository;
    }

   

    public async Task<ErrorOr<GetTransactionsResult>> GetTransactionsAsync(CancellationToken cancellationToken)
    {
        

        var transactionsResult = await _transactionRepository.GetTransactionsAsync(cancellationToken);
        if(transactionsResult.IsError)
        {
            
            return transactionsResult.Errors;

        }
        
        return new GetTransactionsResult
        {

             Transactions = transactionsResult.Value

        };


    }

    public async Task<ErrorOr<Transaction>> GetTransactionByIdAsync (int id, CancellationToken cancellationToken)
    {
        
        var transactionResult = await _transactionRepository.GetTransactionByIdAsync(id, cancellationToken);

        if(transactionResult.IsError)
        {
            
            return transactionResult.Errors;

        }

        return transactionResult;

    }

    public async Task<ErrorOr<Transaction>> CreateTransactionAsync (decimal? price, DateOnly? transactionDate,
    TransactionType transactionType, string? description, string? location, int userId, int categoryId, int paymentMethodId,
    CancellationToken cancellationToken)
    {
        var userResult = await _userRepository.GetUserByIdAsync(userId, cancellationToken);

        var categoryResult  = await _categoryRepository.GetCategoryByIdAsync(categoryId, cancellationToken);

        var paymentMethodResult = await _paymentMethodRepository.GetPaymentMethodByIdAsync(paymentMethodId, cancellationToken);

        if (userResult.IsError)
            {
            return UserErrors.NotFound;
        }

         if (categoryResult.IsError)
            {
            return CategoryErrors.NotFound;
        }

        if (paymentMethodResult.IsError)
            {
            return PaymentMethodErrors.NotFound;
        }
        
        var validationResult = TransactionValidator.ValidateCreateTransactionRequest(price, transactionDate, transactionType,
         description, location, userId, categoryId, paymentMethodId);

         if(validationResult.IsError)
        {
            return validationResult.Errors;

        }

        

        var transaction = new Transaction
        {
            
            Price = price,
            TransactionDate = transactionDate,
            TransactionType = transactionType,
            Description = description,
            Location  = location,
            UserId = userId,
            CategoryId = categoryId,
            PaymentMethodId = paymentMethodId


        };


        var createTransaction = await _transactionRepository.CreateTransactionAsync(transaction, cancellationToken);
        return createTransaction;

    }

    public async Task <ErrorOr<Transaction>> UpdateTransactionAsync(int id, decimal? price, DateOnly? transactionDate,
    TransactionType? transactionType, string? description, string? location, int? categoryId, int? paymentMethodId,
    CancellationToken cancellationToken)
    {

        
        var existing = await _transactionRepository.GetTransactionByIdAsync(id, cancellationToken);
        if (existing.IsError)
            return existing.Errors;

        var old = existing.Value;

        var updated = new Transaction
{
        Id = old.Id,
        UserId = old.UserId,

        Price = price ?? old.Price,
        TransactionDate = transactionDate ?? old.TransactionDate,
        TransactionType = transactionType ?? old.TransactionType,
        Description = description ?? old.Description,
        Location = location ?? old.Location,
        CategoryId = categoryId ?? old.CategoryId,
        PaymentMethodId = paymentMethodId ?? old.PaymentMethodId
};

    if (categoryId.HasValue)
        {
            var categoryResult = await _categoryRepository.GetCategoryByIdAsync(categoryId.Value, cancellationToken);
            if (categoryResult.IsError) return CategoryErrors.NotFound;
        }

    if (paymentMethodId.HasValue)
        {   
            var paymentResult = await _paymentMethodRepository.GetPaymentMethodByIdAsync(paymentMethodId.Value, cancellationToken);
            if (paymentResult.IsError) return PaymentMethodErrors.NotFound;
        }

            var validationResult = TransactionValidator.ValidateUpdateTransactionRequest(
            updated.Price,
            updated.TransactionDate,
            updated.TransactionType,
            updated.Description,
            updated.Location,
            updated.CategoryId,
            updated.PaymentMethodId);


       

    if (validationResult.IsError)
    return validationResult.Errors;

        return await _transactionRepository.UpdateTransactionAsync(updated, cancellationToken);



    }


    public async Task<ErrorOr<bool>> DeleteTransactionAsync(int id, CancellationToken cancellationToken)
    {
        

        var deleteTransaction = await _transactionRepository.DeleteTransactionAsync(id, cancellationToken);
        return deleteTransaction;



    }







}