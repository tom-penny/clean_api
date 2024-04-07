using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Shop.API.Controllers;

using Mappings;
using Extensions;
using Models.Requests;
using Application.Addresses.Commands;
using Application.Addresses.Queries;

[ApiController]
public class AddressController : ControllerBase
{
    private readonly IMediator _mediator;

    public AddressController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [Authorize(Policy = "RequireLogin")]
    [HttpGet("/api/users/{userId}/addresses/{addressId}")]
    public async Task<IActionResult> GetAddress([FromRoute] Guid userId, [FromRoute] Guid addressId,
        CancellationToken cancellationToken)
    {
        var query = new GetAddressByIdQuery(addressId, userId);
        
        var result = await _mediator.Send(query, cancellationToken);
        
        return result.IsSuccess ? Ok(result.Value.ToResponse()) : result.ToProblem();
    }
    
    [Authorize(Policy = "RequireLogin")]
    [HttpGet("/api/users/{userId}/addresses")]
    public async Task<IActionResult> GetAllAddresses([FromRoute] Guid userId,
        CancellationToken cancellationToken)
    {
        var query = new GetAllAddressesQuery(userId);

        var result = await _mediator.Send(query, cancellationToken);

        var response = result.Value.ToResponse();
        
        return Ok(response);
    }
    
    [Authorize(Policy = "RequireLogin")]
    [HttpPost("/api/users/{userId}/addresses")]
    public async Task<IActionResult> CreateAddress([FromRoute] Guid userId,
        [FromBody] CreateAddressRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateAddressCommand
        {
            UserId = userId,
            Street = request.Street,
            City = request.City,
            Country = request.Country,
            PostCode = request.PostCode
        };
        
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailed) return result.ToProblem();
        
        var address = result.Value.ToResponse();

        return CreatedAtAction(nameof(GetAddress), new { userId = address.UserId, addressId = address.Id }, address);
    }
    
    [Authorize(Policy = "RequireLogin")]
    [HttpDelete("/api/users/{userId}/addresses/{addressId}")]
    public async Task<IActionResult> DeleteAddress([FromRoute] Guid userId, Guid addressId,
        CancellationToken cancellationToken)
    {
        var command = new DeleteAddressCommand(addressId, userId);

        var result = await _mediator.Send(command, cancellationToken);
        
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}