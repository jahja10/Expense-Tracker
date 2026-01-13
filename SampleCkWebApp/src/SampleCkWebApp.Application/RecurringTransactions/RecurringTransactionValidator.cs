using ErrorOr;
using SampleCkWebApp.Domain.Errors;
using SampleCkWebApp.Domain.Enums;
using System.Text.RegularExpressions;

namespace SampleCkWebApp.Application.RecurringTransactions;



public static class RecurringTransactionValidator {
public static ErrorOr<Success> ValidateCreateRecurringTransactionRequest(string name, FrequencyOfTransaction 
    frequencyOfTransaction, DateOnly? nextRunDate, int userId, int categoryId, 
    int paymentMethodId)
{

    if(categoryId <= 0 || userId <= 0 || paymentMethodId <= 0)
        {
            
            return TransactionErrors.ForeignKeyMissing;

        }

    if(!Enum.IsDefined(typeof(FrequencyOfTransaction), frequencyOfTransaction))
        {
            return TransactionErrors.InvalidTransactionType;

        }

        if (string.IsNullOrWhiteSpace(name) || name.Length > 100)
    {
        
        return CategoryErrors.InvalidName;

    }

    if (Regex.IsMatch(name, @"\d"))
            return RecurringTransactionErrors.NameContainsNumbers;

    

        return Result.Success;


}


    public static ErrorOr<Success> ValidateUpdateRecurringTransactionRequest(string name, FrequencyOfTransaction 
    frequencyOfTransaction, DateOnly? nextRunDate, int categoryId, 
    int paymentMethodId)
{

    if(categoryId <= 0 || paymentMethodId <= 0)
        {
            
            return TransactionErrors.ForeignKeyMissing;

        }

    if(!Enum.IsDefined(typeof(FrequencyOfTransaction), frequencyOfTransaction))
        {
            return TransactionErrors.InvalidTransactionType;

        }

         if (string.IsNullOrWhiteSpace(name) || name.Length > 100)
    {
        
        return CategoryErrors.InvalidName;

    }

     if (Regex.IsMatch(name, @"\d"))
            return CategoryErrors.NameContainsNumbers;
    

    
        return Result.Success;


}


}