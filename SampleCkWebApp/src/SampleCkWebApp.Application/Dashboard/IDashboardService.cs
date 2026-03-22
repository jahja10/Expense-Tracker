using ErrorOr;
using SampleCkWebApp.Contracts.Dashboard;

namespace SampleCkWebApp.Application.Dashboard;

public interface IDashboardService
{
    Task<ErrorOr<DashboardSummaryResponse>> GetSummaryAsync(int userId, CancellationToken cancellationToken);

    Task <ErrorOr<List<DashboardSpendingTrendItemResponse>>> GetSpendingTrendAsync (int userId, CancellationToken cancellationToken);

    Task<ErrorOr<List<DashboardTransactionsResponse>>> GetRecentTransactionsAsync( int userId, CancellationToken cancellationToken);
}