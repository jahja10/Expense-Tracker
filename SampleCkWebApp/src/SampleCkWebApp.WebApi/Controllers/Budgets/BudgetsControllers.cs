using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SampleCkWebApp.Application.Budgets.Interfaces.Application;
using SampleCkWebApp.Contracts.Budgets;
using SampleCkWebApp.WebApi.Mappings;

namespace SampleCkWebApp.WebApi.Controllers.Budgets;

[Authorize]
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class BudgetsController : ApiControllerBase
{
    private readonly IBudgetService _budgetService;

    public BudgetsController(IBudgetService budgetService)
    {
        _budgetService = budgetService;
    }

    // GET /budgets/active
    [HttpGet("active")]
    [ProducesResponseType(typeof(BudgetResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetActiveBudget(CancellationToken cancellationToken)
    {
        var userId = CurrentUserId;

        var result = await _budgetService.GetActiveBudgetByUserIdAsync(
            userId,
            cancellationToken
        );

        if (result.IsError)
            return Problem(result.Errors);

        if (result.Value is null)
            return NotFound();

        return Ok(result.Value.ToResponse());
    }

    // POST /budgets
    [HttpPost]
    [ProducesResponseType(typeof(BudgetResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateBudget(
        [FromBody] CreateBudgetRequest request,
        CancellationToken cancellationToken)
    {
        var userId = CurrentUserId;
        var startDate = DateOnly.FromDateTime(DateTime.Today);

        var result = await _budgetService.CreateBudgetAsync(
            request.Amount,
            request.Savings,
            startDate,
            request.EndDate,
            userId,
            cancellationToken
        );

        if (result.IsError)
            return Problem(result.Errors);

        return CreatedAtAction(
            nameof(GetActiveBudget),
            new { },
            result.Value.ToResponse()
        );
    }

   
    [HttpPost("{id:int}/close")]
    [ProducesResponseType(typeof(BudgetResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CloseBudget(
        [FromRoute] int id,
        [FromBody] CloseBudgetRequest request,
        CancellationToken cancellationToken)
    {
        var userId = CurrentUserId;

        var result = await _budgetService.CloseBudgetAsync(
            id,
            request.EndDate,
            userId,
            cancellationToken
        );

        if (result.IsError)
            return Problem(result.Errors);

        return Ok(result.Value.ToResponse());
    }
}
