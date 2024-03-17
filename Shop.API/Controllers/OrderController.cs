using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Shop.API.Controllers;

using Mappings;
using Extensions;
using Models.Requests;
using Application.Orders.Commands;
using Application.Orders.Queries;

[ApiController]
public class OrderController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize(Policy = "RequireLogin")]
    [HttpGet("/api/users/{userId}/orders")]
    public async Task<IActionResult> GetAllOrders([FromRoute] Guid userId, [FromBody] GetAllOrdersRequest request,
        CancellationToken cancellationToken)
    {
        var query = new GetAllOrdersQuery
        {
            UserId = userId,
            SortBy = request.SortBy,
            OrderBy = request.OrderBy,
            Page = request.Page,
            Size = request.Size
        };
        
        var result = await _mediator.Send(query, cancellationToken);

        var response = result.Value.ToResponse();
        
        return Ok(response);
    }

    [Authorize(Policy = "RequireLogin")]
    [HttpGet("/api/users/{userId}/orders/{orderId}")]
    public async Task<IActionResult> GetOrder([FromRoute] Guid userId, [FromRoute] Guid orderId,
        CancellationToken cancellationToken)
    {
        var query = new GetOrderByIdQuery(orderId, userId);
        
        var result = await _mediator.Send(query, cancellationToken);
        
        return result.IsSuccess ? Ok(result.Value.ToResponse()) : result.ToProblem();
    }
    
    [Authorize(Policy = "RequireLogin")]
    [HttpPost("/api/orders")]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateOrderCommand
        {
            CheckoutId = request.CheckoutId,
            UserId = request.UserId,
            Amount = request.Amount,
            Items = request.Items
                .Select(item => new CreateOrderItem
                (
                    item.ProductId,
                    item.Quantity,
                    item.UnitPrice
                ))
                .ToList()
        };
        
        var result = await _mediator.Send(command, cancellationToken);
        
        if (result.IsFailed) return result.ToProblem();
        
        var order = result.Value.ToResponse();

        return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
    }
}