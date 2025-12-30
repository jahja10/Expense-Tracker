using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using SampleCkWebApp.Transactions;
using SampleCkWebApp.Application.Transactions.Interfaces.Application;
using SampleCkWebApp.Application.Transactions.Mappings;
using System;



namespace SampleCkWebApp.WebApi.Controllers.Transactions;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class TransactionsController : ApiControllerBase
{
    

private readonly ITransactionService _transactionService;

    public TransactionsController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(GetTransactionsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetTransactions(CancellationToken cancellationToken)
    {
        var result = await _transactionService.GetTransactionsAsync(cancellationToken);

        return result.Match(
            transactionsResult => Ok(transactionsResult.ToResponse()),
            Problem
        );
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetTransactionById([FromRoute, Required] int id, CancellationToken cancellationToken)
    {
        var result = await _transactionService.GetTransactionByIdAsync(id, cancellationToken);

        return result.Match(
            transaction => Ok(transaction.ToResponse()),
            Problem
        );
    }

    [HttpPost]
    [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateTransaction(
    [FromBody, Required] CreateTransactionRequest request,
    CancellationToken cancellationToken)
    {
       var result = await _transactionService.CreateTransactionAsync(
        request.Price,
        request.TransactionDate,
        (SampleCkWebApp.Domain.Enums.TransactionType)Enum.Parse(
        typeof(SampleCkWebApp.Domain.Enums.TransactionType),
            request.TransactionType.ToString(),
            ignoreCase: true),

            request.Description,
            request.Location,
            request.UserId,
            request.CategoryId,
            request.PaymentMethodId,
            cancellationToken
        );


        return result.Match(
        transaction => CreatedAtAction(
            nameof(GetTransactionById),
            new { id = transaction.Id },
            transaction.ToResponse()
        ),
        Problem
    );
}

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateTransaction(
    [FromRoute, Required] int id,
    [FromBody, Required] UpdateTransactionRequest request,
    CancellationToken cancellationToken)
    {
        var transactionType = request.TransactionType is null
        ? (SampleCkWebApp.Domain.Enums.TransactionType?)null
        : Enum.Parse<SampleCkWebApp.Domain.Enums.TransactionType>(
        request.TransactionType.Value.ToString(),
        ignoreCase: true);

        var result = await _transactionService.UpdateTransactionAsync(
            id,
            request.Price,
            request.TransactionDate,
            transactionType,               
            request.Description,
            request.Location,
            request.CategoryId,            
            request.PaymentMethodId,       
            cancellationToken
);



    return result.Match(
        transaction => Ok(transaction.ToResponse()),
        Problem
    );
}

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteTransaction(
    [FromRoute, Required] int id,
    CancellationToken cancellationToken)
{
    var result = await _transactionService.DeleteTransactionAsync(id, cancellationToken);

    return result.Match(
        _ => NoContent(),
        Problem
    );
}










}