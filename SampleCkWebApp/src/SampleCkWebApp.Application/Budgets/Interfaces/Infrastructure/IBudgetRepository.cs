using ErrorOr;
using SampleCkWebApp.Domain.Entities;

namespace SampleCkWebApp.Application.Budgets.Interfaces.Infrastructure;


public interface IBudgetRepository
{
    

   

    Task<ErrorOr<Budget?>> GetActiveBudgetByUserIdAsync(int userId, CancellationToken cancellationToken);
    
    Task<ErrorOr<Budget>> CreateBudgetAsync(Budget budget, CancellationToken cancellationToken);

    Task<ErrorOr<Budget>> CloseBudgetAsync(int id, DateOnly endDate, int userId, CancellationToken cancellationToken);



}