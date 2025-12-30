using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using SampleCkWebApp.RecurringTransactions;
using SampleCkWebApp.Application.RecurringTransactions.Interfaces.Application;
using SampleCkWebApp.Application.RecurringTransactions.Mappings;
using System;



namespace SampleCkWebApp.WebApi.Controllers.RecurringTransactions;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class RecurringTransactionsController : ApiControllerBase
{
    

private readonly IRecurringTransactionService _recurringTransactionService;

    public RecurringTransactionsController(IRecurringTransactionService recurringTransactionService)
    {
        _recurringTransactionService = recurringTransactionService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(GetRecurringTransactionsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetRecurringTransactions(CancellationToken cancellationToken)
    {
        var result = await _recurringTransactionService.GetRecurringTransactionsAsync(cancellationToken);

        return result.Match(
            recurringTransactionsResult => Ok(recurringTransactionsResult.ToResponse()),
            Problem
        );
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(RecurringTransactionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetRecurringTransactionById([FromRoute, Required] int id, CancellationToken cancellationToken)
    {
        var result = await _recurringTransactionService.GetRecurringTransactionByIdAsync(id, cancellationToken);

        return result.Match(
            recurringTransaction => Ok(recurringTransaction.ToResponse()),
            Problem
        );
    }

    [HttpPost]
    [ProducesResponseType(typeof(RecurringTransactionResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateRecurringTransaction(
    [FromBody, Required] CreateRecurringTransactionRequest request,
    CancellationToken cancellationToken)
    {

       var frequencyOfTransaction = Enum.Parse<SampleCkWebApp.Domain.Enums.FrequencyOfTransaction>(
       request.FrequencyOfTransaction.ToString(),
       ignoreCase: true);
       var result = await _recurringTransactionService.CreateRecurringTransactionAsync(

        request.Name,
        frequencyOfTransaction,
        request.NextRunDate,
        request.UserId,
        request.CategoryId,
        request.PaymentMethodId,
        cancellationToken
        );


        return result.Match(
        recurringTransaction => CreatedAtAction(
            nameof(GetRecurringTransactionById),
            new { id = recurringTransaction.Id},
            recurringTransaction.ToResponse()
        ),
        Problem
    );
}

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(RecurringTransactionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateRecurringTransaction(
    [FromRoute, Required] int id,
    [FromBody, Required] UpdateRecurringTransactionRequest request,
    CancellationToken cancellationToken)
    {
        var frequencyOfTransaction = request.FrequencyOfTransaction is null
        ? (SampleCkWebApp.Domain.Enums.FrequencyOfTransaction?)null
        : Enum.Parse<SampleCkWebApp.Domain.Enums.FrequencyOfTransaction>(
        request.FrequencyOfTransaction.Value.ToString(),
        ignoreCase: true);

        var result = await _recurringTransactionService.UpdateRecurringTransactionAsync(
            id,
            request.Name,
            frequencyOfTransaction,
            request.NextRunDate,          
            request.CategoryId,            
            request.PaymentMethodId,       
            cancellationToken
);



    return result.Match(
        recurringTransaction => Ok(recurringTransaction.ToResponse()),
        Problem
    );
}

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteRecurringTransaction(
    [FromRoute, Required] int id,
    CancellationToken cancellationToken)
{
    var result = await _recurringTransactionService.DeleteRecurringTransactionAsync(id, cancellationToken);

    return result.Match(
        _ => NoContent(),
        Problem
    );
}


}