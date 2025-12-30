using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using SampleCkWebApp.PaymentMethods;
using SampleCkWebApp.Application.PaymentMethods.Interfaces.Application;
using SampleCkWebApp.Application.PaymentMethods.Mappings;

namespace SampleCkWebApp.WebApi.Controllers.PaymentMethods;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class PaymentMethodsController : ApiControllerBase
{
    private readonly IPaymentMethodService _paymentmethodService;

    public PaymentMethodsController(IPaymentMethodService paymentmethodService)
    {
        _paymentmethodService = paymentmethodService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(GetPaymentMethodsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPaymentMethods(CancellationToken cancellationToken)
    {
        var result = await _paymentmethodService.GetPaymentMethodsAsync(cancellationToken);

        return result.Match(
            paymentMethodsResult => Ok(paymentMethodsResult.ToResponse()),
            Problem
        );
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PaymentMethodResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetPaymentMethodById([FromRoute, Required] int id, CancellationToken cancellationToken)
    {
        var result = await _paymentmethodService.GetPaymentMethodByIdAsync(id, cancellationToken);

        return result.Match(
            paymentMethod => Ok(paymentMethod.ToResponse()),
            Problem
        );
    }

    [HttpPost]
    [ProducesResponseType(typeof(PaymentMethodResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreatePaymentMethod([FromBody, Required] CreatePaymentMethodRequest request, CancellationToken cancellationToken)
    {
        var result = await _paymentmethodService.CreatePaymentMethodAsync(request.Name, cancellationToken);

        return result.Match(
            paymentMethod => CreatedAtAction(
                nameof(GetPaymentMethodById),
                new { id = paymentMethod.Id },
                paymentMethod.ToResponse()
            ),
            Problem
        );
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(PaymentMethodResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdatePaymentMethod([FromRoute, Required] int id, [FromBody, Required] UpdatePaymentMethodRequest request, CancellationToken cancellationToken)
    {
        var result = await _paymentmethodService.UpdatePaymentMethodAsync(id, request.Name, cancellationToken);

        return result.Match(
            paymentMethod => Ok(paymentMethod.ToResponse()),
            Problem
        );
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeletePaymentMethod([FromRoute, Required] int id, CancellationToken cancellationToken)
    {
        var result = await _paymentmethodService.DeletePaymentMethodAsync(id, cancellationToken);

        return result.Match(
            _ => NoContent(),
            Problem
        );
    }
}
