using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Shop.API.Controllers;

using Mappings;
using Extensions;
using Models.Requests;
using Application.Shipments.Commands;
using Application.Shipments.Queries;

[ApiController]
public class ShipmentController : ControllerBase
{
    private readonly IMediator _mediator;

    public ShipmentController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [Authorize(Policy = "RequireAdmin")]
    [HttpGet("/api/shipments/{id}")]
    public async Task<IActionResult> GetShipment([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query = new GetShipmentByIdQuery(id);
        
        var result = await _mediator.Send(query, cancellationToken);
        
        return result.IsSuccess ? Ok(result.Value.ToResponse()) : result.ToProblem();
    }
    
    [Authorize(Policy = "RequireAdmin")]
    [HttpPost("/api/shipments")]
    public async Task<IActionResult> CreateShipment([FromBody] CreateShipmentRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateShipmentCommand(request.OrderId);

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailed) return result.ToProblem();
        
        var shipment = result.Value.ToResponse();

        return CreatedAtAction(nameof(GetShipment), new { id = shipment.Id }, shipment);
    }
    
    [Authorize(Policy = "RequireAdmin")]
    [HttpPut("/api/shipments/{id}")]
    public async Task<IActionResult> UpdateShipment([FromRoute] Guid id,
        [FromBody] UpdateShipmentRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateShipmentCommand(id, request.DeliveryDate);
        
        var result = await _mediator.Send(command, cancellationToken);
        
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
}