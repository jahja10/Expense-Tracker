using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SampleCkWebApp.Application.Dashboard;


namespace SampleCkWebApp.WebApi.Controllers;

[ApiController]
[Route("api/dashboard")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet("summary")]
    public async Task<IActionResult> Summary(CancellationToken cancellationToken)
    {
        var userIdStr =
            User.FindFirstValue(ClaimTypes.NameIdentifier) ??
            User.FindFirstValue("sub") ??
            User.FindFirstValue("userId");

        if (!int.TryParse(userIdStr, out var userId))
            return Unauthorized("Missing/invalid user id claim in JWT.");

        var result = await _dashboardService.GetSummaryAsync(userId, cancellationToken);

        if (result.IsError)
            return Problem(title: result.FirstError.Description);

        return Ok(result.Value);
    }

    [HttpGet("spending-trend")]
    public async Task<IActionResult> GetSpendingTrend(CancellationToken cancellationToken)
    {
        
        var userIdStr = 
        User.FindFirstValue(ClaimTypes.NameIdentifier) ??
        User.FindFirstValue("sub") ?? 
        User.FindFirstValue("userId");

        if(!int.TryParse(userIdStr, out var userId))
        return Unauthorized("Missing/Invalid user id claim in JWT.");

        var result = await _dashboardService.GetSpendingTrendAsync(userId, cancellationToken);

        if (result.IsError)
        {
            
            return Problem(title: result.FirstError.Description);
        }

        return Ok(result.Value);



    }

    [HttpGet("recent-transactions")]
public async Task<IActionResult> GetRecentTransactions(CancellationToken cancellationToken)
{
    var userIdStr =
        User.FindFirstValue(ClaimTypes.NameIdentifier) ??
        User.FindFirstValue("sub") ??
        User.FindFirstValue("userId");

    if (!int.TryParse(userIdStr, out var userId))
        return Unauthorized("Missing/invalid user id claim in JWT.");

    var result = await _dashboardService.GetRecentTransactionsAsync(userId, cancellationToken);

    if (result.IsError)
        return Problem(title: result.FirstError.Description);

    return Ok(result.Value);
}
}