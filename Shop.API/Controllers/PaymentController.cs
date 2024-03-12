using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Shop.API.Controllers;

using Mappings;
using Extensions;
using Models.Requests;
using Application.Payments.Commands;
using Application.Payments.Queries;

[ApiController]
public class PaymentController : ControllerBase
{
    private readonly IMediator _mediator;

    public PaymentController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [Authorize(Policy = "RequireAdmin")]
    [HttpGet("/api/payments/{id}")]
    public async Task<IActionResult> GetPayment([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query = new GetPaymentByIdQuery(id);
        
        var result = await _mediator.Send(query, cancellationToken);
        
        return result.IsSuccess ? Ok(result.Value.ToResponse()) : result.ToProblem();
    }
    
    [HttpPost("/api/payments")]
    public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreatePaymentCommand
        {
            CheckoutId = request.CheckoutId,
            Status = request.Status,
            Amount = request.Amount
        };
        
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailed) return result.ToProblem();
        
        var payment = result.Value.ToResponse();

        return CreatedAtAction(nameof(GetPayment), new { id = payment.Id }, payment);
    }
}