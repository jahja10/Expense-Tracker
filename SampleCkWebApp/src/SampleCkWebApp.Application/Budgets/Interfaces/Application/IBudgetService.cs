using ErrorOr;
using SampleCkWebApp.Domain.Entities;


namespace SampleCkWebApp.Application.Budgets.Interfaces.Application;


public interface IBudgetService
{
    



    Task<ErrorOr<Budget?>> GetActiveBudgetByUserIdAsync(int userId, CancellationToken cancellationToken);
    
    Task<ErrorOr<Budget>> CreateBudgetAsync(decimal amount, decimal? savings, DateOnly startDate, 
    DateOnly? endDate, int userId,  CancellationToken cancellationToken);

    Task<ErrorOr<Budget>> CloseBudgetAsync(int id, DateOnly endDate, int userId, CancellationToken cancellationToken);



   

    



}