using ErrorOr;
using SampleCkWebApp.Domain.Errors;
using SampleCkWebApp.Domain.Enums;
using System.Text.RegularExpressions;

namespace SampleCkWebApp.Application.RecurringTransactions;



public static class RecurringTransactionValidator {
public static ErrorOr<Success> ValidateCreateRecurringTransactionRequest(string name, FrequencyOfTransaction 
    frequencyOfTransaction, DateOnly startDate, int userId, int categoryId, 
    int paymentMethodId, decimal amount)
{

    if(categoryId <= 0 || userId <= 0 || paymentMethodId <= 0 || amount <= 0)
        {
            
            return TransactionErrors.ForeignKeyMissing;

        }

    if(!Enum.IsDefined(typeof(FrequencyOfTransaction), frequencyOfTransaction))
        {
            return TransactionErrors.InvalidTransactionType;

        }

        if (string.IsNullOrWhiteSpace(name) || name.Length > 100)
    {
        
        return RecurringTransactionErrors.InvalidName;

    }

    if (Regex.IsMatch(name, @"\d"))
            return RecurringTransactionErrors.NameContainsNumbers;

    

        return Result.Success;


}


    public static ErrorOr<Success> ValidateUpdateRecurringTransactionRequest(string name, FrequencyOfTransaction 
    frequencyOfTransaction, DateOnly startDate, int categoryId, 
    int paymentMethodId, decimal amount)
{

    if(categoryId <= 0 || paymentMethodId <= 0 || amount <= 0)
        {
            
            return TransactionErrors.ForeignKeyMissing;

        }

    if(!Enum.IsDefined(typeof(FrequencyOfTransaction), frequencyOfTransaction))
        {
            return TransactionErrors.InvalidTransactionType;

        }

         if (string.IsNullOrWhiteSpace(name) || name.Length > 100)
    {
        
        return RecurringTransactionErrors.InvalidName;

    }

     if (Regex.IsMatch(name, @"\d"))
            return RecurringTransactionErrors.NameContainsNumbers;
    

    
        return Result.Success;


}


}