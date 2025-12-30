using ErrorOr;
using SampleCkWebApp.Domain.Entities;
using SampleCkWebApp.Domain.Errors;
using SampleCkWebApp.Application.Budgets.Interfaces.Application;
using SampleCkWebApp.Application.Budgets.Interfaces.Infrastructure;

namespace SampleCkWebApp.Application.Budgets;


public class BudgetService : IBudgetService
{
    
    private readonly IBudgetRepository _budgetRepository;

    public BudgetService(IBudgetRepository budgetRepository)
    {
        
        _budgetRepository = budgetRepository;

    }


   public async Task <ErrorOr<Budget?>> GetActiveBudgetByUserIdAsync(int userId, CancellationToken cancellationToken)
    {

        var result = await _budgetRepository.GetActiveBudgetByUserIdAsync(userId, cancellationToken);

        if (result.IsError)
        return result.Errors;

    
    return result;


    }

    public async Task <ErrorOr<Budget>> CreateBudgetAsync(decimal amount, decimal? savings, DateOnly startDate, 
    DateOnly? endDate, int userId,  CancellationToken cancellationToken)
    {
        

        var validateBudget = BudgetValidator.ValidateCreateBudgetRequest(userId, amount, startDate, endDate);

        if (validateBudget.IsError)
        {
            
            return validateBudget.Errors;

        }


        var budget = new Budget
        {
            
            Amount = amount,
            Savings = savings,
            StartDate = startDate,
            EndDate = endDate,
            UserId = userId



        }; 

        var createBudget = await _budgetRepository.CreateBudgetAsync(budget, cancellationToken);
        return createBudget;



    }


    public async Task <ErrorOr<Budget>>  CloseBudgetAsync (int id, DateOnly endDate, int userId, CancellationToken cancellationToken)
    {
        

        var validateBudget = BudgetValidator.ValidateCloseBudgetRequest(id, userId);

        if (validateBudget.IsError)
        {

                return validateBudget.Errors;

        }

        var closeBudget = await _budgetRepository.CloseBudgetAsync(id, endDate, userId, cancellationToken);
        return closeBudget;
        
    }    


}