using ErrorOr;
using SampleCkWebApp.Application.Budgets.Interfaces.Infrastructure;
using SampleCkWebApp.Application.Transactions.Interfaces.Infrastructure;
using SampleCkWebApp.Application.RecurringTransactions.Interfaces.Infrastructure;
using SampleCkWebApp.Application.Users.Interfaces.Infrastructure;
using SampleCkWebApp.Contracts.Dashboard;
using SampleCkWebApp.Domain.Enums;
using SampleCkWebApp.Application.RecurringToTransaction;
using SampleCkWebApp.Domain.Entities;

namespace SampleCkWebApp.Application.Dashboard;

public class DashboardService : IDashboardService
{
    private readonly IUserRepository _users;
    private readonly ITransactionRepository _transactions;
    private readonly IBudgetRepository _budgets;
    private readonly IRecurringTransactionRepository _recurringTransactions;

    private readonly IRecurringToTransactionService _recurringToTransactionService;

   

    public DashboardService(
        IUserRepository users,
        ITransactionRepository transactions,
        IBudgetRepository budgets,
        IRecurringTransactionRepository recurringTransaction,
        IRecurringToTransactionService recurringToTransactionService
        )
    {
        _users = users;
        _transactions = transactions;
        _budgets = budgets;
        _recurringTransactions = recurringTransaction;
        _recurringToTransactionService = recurringToTransactionService;
       
    }

    public async Task<ErrorOr<DashboardSummaryResponse>> GetSummaryAsync(
        int userId,
        CancellationToken cancellationToken)
    {
        var userResult = await _users.GetUserByIdAsync(userId, cancellationToken);
        if (userResult.IsError)
            return Error.NotFound(description: "User not found");

        var processRecurringResult = await _recurringToTransactionService .ProcessRecurringTransactionsAsync(userId, cancellationToken);

        if (processRecurringResult.IsError)
        return Error.Failure(description: "Failed to process recurring transactions");    

        var txResult = await _transactions.GetTransactionsAsync(userId, cancellationToken);
        if (txResult.IsError)
            return Error.Failure(description: "Failed to load transactions");

        var today = DateOnly.FromDateTime(DateTime.Today);
        var monthStart = new DateOnly(today.Year, today.Month, 1);
        var monthEndExclusive = monthStart.AddMonths(1);

        decimal spentThisMonth = txResult.Value
            .Where(t => t.TransactionDate is not null)
            .Where(t => t.TransactionDate >= monthStart && t.TransactionDate < monthEndExclusive)
            .Where(t => t.TransactionType == TransactionType.Expense)
            .Sum(t => t.Price ?? 0m);

        int totalTransactionsThisMonth = txResult.Value
        .Where(t => t.TransactionDate is not null)
        .Where(t => t.TransactionDate >= monthStart && t.TransactionDate < monthEndExclusive)
        .Count();    

        var budgetResult = await _budgets.GetActiveBudgetByUserIdAsync(userId, cancellationToken);

        decimal monthlyBudget = 0m;
        DateOnly? budgetEndDate = null;

        if (!budgetResult.IsError && budgetResult.Value is not null)
        {
            monthlyBudget = budgetResult.Value.Amount;
            budgetEndDate = budgetResult.Value.EndDate;
        }

        var recurringTr = await _recurringTransactions.GetRecurringTransactionsAsync(userId, cancellationToken);
        if (recurringTr.IsError)
            return Error.Failure(description: "Failed to load recurring transactions");

        decimal upcomingPaymentsThisMonth = recurringTr.Value
        .Where(t => t.IsActive)
        .Select(t => new
        {
            Transaction = t,
            NextDueDate = CalculateNextDueDate(t)
        })
        .Where(x => x.NextDueDate >= monthStart && x.NextDueDate < monthEndExclusive)
        .Sum(x => x.Transaction.Amount);
        
        int daysPassed = today.Day;

        decimal averageDailySpending = daysPassed > 0
            ? spentThisMonth / daysPassed
            : 0m;


        var thisMonthRecurring = recurringTr.Value
            .Where(t => t.IsActive)
            .Select(t => new
            {
               Transaction = t,
               NextDueDate = CalculateNextDueDate(t)
            })
            .Where(x => x.NextDueDate >= today)
            .OrderBy(x => x.NextDueDate)
            .Take(5)
            .Select(x => new DashboardRecurringTransactionsResponse(
                x.Transaction.Name,
                x.Transaction.Amount,
                x.NextDueDate
            ))
            .ToList();

        return new DashboardSummaryResponse(
            userResult.Value.Name,
            spentThisMonth,
            monthlyBudget,
            budgetEndDate,
            thisMonthRecurring,
            totalTransactionsThisMonth,
            upcomingPaymentsThisMonth,
            averageDailySpending
            
        );
    }

        public async Task<ErrorOr<List<DashboardSpendingTrendItemResponse>>> GetSpendingTrendAsync( int userId, CancellationToken cancellationToken)
    {
        var userResult = await _users.GetUserByIdAsync(userId, cancellationToken);
        if (userResult.IsError)
            return Error.NotFound(description: "User not found");

        var txResult = await _transactions.GetTransactionsAsync(userId, cancellationToken);
        if (txResult.IsError)
            return Error.Failure(description: "Failed to load transactions");

        var budgetResult = await _budgets.GetActiveBudgetByUserIdAsync(userId, cancellationToken);

        decimal monthlyBudget = 0m;
        if (!budgetResult.IsError && budgetResult.Value is not null)
        {
            monthlyBudget = budgetResult.Value.Amount;
        }

        var today = DateOnly.FromDateTime(DateTime.Today);
        var currentMonthStart = new DateOnly(today.Year, today.Month, 1);

        var result = new List<DashboardSpendingTrendItemResponse>();

        for (int i = 5; i >= 0; i--)
        {
            var monthStart = currentMonthStart.AddMonths(-i);
            var monthEnd = monthStart.AddMonths(1);

            decimal spent = txResult.Value
                .Where(t => t.TransactionDate is not null)
                .Where(t => t.TransactionDate >= monthStart && t.TransactionDate < monthEnd)
                .Where(t => t.TransactionType == TransactionType.Expense)
                .Sum(t => t.Price ?? 0m);

            result.Add(new DashboardSpendingTrendItemResponse(
                monthStart
                .ToDateTime(TimeOnly.MinValue)
                .ToString("MMM", System.Globalization.CultureInfo.InvariantCulture),
                spent,
                monthlyBudget
            ));
        }

        return result;
    }


   public async Task<ErrorOr<List<DashboardTransactionsResponse>>> GetRecentTransactionsAsync(
    int userId,
    CancellationToken cancellationToken)
{
    var userResult = await _users.GetUserByIdAsync(userId, cancellationToken);

    if (userResult.IsError)
    {
        return Error.NotFound(description: "User not found");
    }

    var recentTransactionsResult = await _transactions.GetRecentTransactionsAsync(
        userId,
        cancellationToken);

    if (recentTransactionsResult.IsError)
{
    return recentTransactionsResult.Errors;
}

    return recentTransactionsResult.Value;
}


        private static DateOnly CalculateNextDueDate(
        RecurringTransaction recurring)
    {
        var baseDate = recurring.LastGeneratedDate ?? recurring.StartDate;

        return recurring.FrequencyOfTransaction switch
        {
            FrequencyOfTransaction.Monthly => baseDate.AddMonths(1),
            FrequencyOfTransaction.Yearly => baseDate.AddYears(1),
            _ => recurring.StartDate
        };
    }
    }