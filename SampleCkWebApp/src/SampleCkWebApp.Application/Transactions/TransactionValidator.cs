using ErrorOr;
using SampleCkWebApp.Domain.Errors;
using SampleCkWebApp.Domain.Enums;

namespace SampleCkWebApp.Application.Transactions;



public static class TransactionValidator {
public static ErrorOr<Success> ValidateCreateTransactionRequest(decimal? price, DateOnly? transactionDate, 
TransactionType transactionType, string? description, string? location, int userId, int categoryId, int paymentMethodId)
{

    if(categoryId <= 0 || userId <= 0 || paymentMethodId <= 0)
        {
            
            return TransactionErrors.ForeignKeyMissing;

        }

    if(!Enum.IsDefined(typeof(TransactionType), transactionType))
        {
            return TransactionErrors.InvalidTransactionType;

        }
    

    if (price is not null)
        {
            if (price <= 0 || price > 99999999.99m)
            {
                return TransactionErrors.InvalidPrice;

            }
                
        }


    if (description is not null && description.Length > 255)
        {
            
            return TransactionErrors.DescriptionTooLong;
        }
        

    if (location is not null && location.Length > 55)
        {
            
            return TransactionErrors.LocationTooLong;

        }
        

        return Result.Success;


}


    public static ErrorOr<Success> ValidateUpdateTransactionRequest(decimal? price, DateOnly? transactionDate, 
TransactionType transactionType, string? description, string? location, int categoryId, int paymentMethodId)
{

    if(categoryId <= 0 || paymentMethodId <= 0)
        {
            
            return TransactionErrors.ForeignKeyMissing;

        }

    if(!Enum.IsDefined(typeof(TransactionType), transactionType))
        {
            return TransactionErrors.InvalidTransactionType;

        }
    

    if (price is not null)
        {
            if (price <= 0 || price > 99999999.99m)
            {
                return TransactionErrors.InvalidPrice;

            }
                
        }

       if (description is not null && description.Length > 255)
        {
            
            return TransactionErrors.DescriptionTooLong;
        }
        

    if (location is not null && location.Length > 55)
        {
            
            return TransactionErrors.LocationTooLong;

        }

        return Result.Success;


}


}