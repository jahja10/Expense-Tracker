using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using SampleCkWebApp.Contracts.Budgets;
using SampleCkWebApp.WebApi.Mappings;
using SampleCkWebApp.Application.Budgets.Interfaces.Application;


namespace SampleCkWebApp.WebApi.Controllers.Budgets;

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

    [HttpGet("{userId:int}")]
    [ProducesResponseType(typeof(BudgetResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetActiveBudgetByUserId(
        [FromRoute, Required] int userId,
        CancellationToken cancellationToken)
    {
         var result = await _budgetService.GetActiveBudgetByUserIdAsync(userId, cancellationToken);

        if (result.IsError)
            return Problem(result.Errors);

        if (result.Value is null)
            return NotFound();

        return Ok(result.Value.ToResponse());

    }


    [HttpPost("{userId:int}")]
    [ProducesResponseType(typeof(BudgetResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateBudget(
        [FromRoute] int userId,
        [FromBody] CreateBudgetRequest request,
        CancellationToken cancellationToken)
    {
       
        var startDate = DateOnly.FromDateTime(DateTime.Today);

        var result = await _budgetService.CreateBudgetAsync(
            request.Amount,
            request.Savings,
            startDate,
            request.EndDate,
            userId,
            cancellationToken);

        if (result.IsError)
            return Problem(result.Errors);

       
        return CreatedAtAction(
            nameof(GetActiveBudgetByUserId),
            new { userId },
            result.Value.ToResponse());
    }

    [HttpPost("{id:int}/close/{userId:int}")]
    [ProducesResponseType(typeof(BudgetResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CloseBudget(
        [FromRoute] int id,
        [FromRoute] int userId,
        [FromBody] CloseBudgetRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _budgetService.CloseBudgetAsync(
            id,
            request.EndDate,
            userId,
            cancellationToken);

        if (result.IsError)
            return Problem(result.Errors);

        return Ok(result.Value.ToResponse());
    }
}









