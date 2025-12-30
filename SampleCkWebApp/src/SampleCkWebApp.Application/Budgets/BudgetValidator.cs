using ErrorOr;
using SampleCkWebApp.Domain.Entities;
using SampleCkWebApp.Domain.Errors;

namespace SampleCkWebApp.Application.Budgets;



public static class BudgetValidator {
public static ErrorOr<Success> ValidateCreateBudgetRequest(int userId, decimal amount, DateOnly startDate, DateOnly? endDate)
{

    if(userId <= 0)
        {
            
            return BudgetErrors.ForeignKeyMissing;

        }

        if(amount < 100)
        {
            
            return BudgetErrors.InvalidAmount;
        }


        if(endDate is not null && endDate.Value <= startDate)
        {

            return BudgetErrors.InvalidDate;

        }

    return Result.Success;


}


public static ErrorOr<Success> ValidateCloseBudgetRequest(int id, int userId)
    {
        


        if(userId <= 0)
        {
            
            return BudgetErrors.ForeignKeyMissing;

        }

        if(id <= 0)
        {
            
            return BudgetErrors.ForeignKeyMissing;

        }

    return Result.Success;

    }

}