using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Shop.API.Controllers;

using Extensions;
using Mappings;
using Models.Requests;
using Application.Users.Commands;
using Application.Users.Queries;

[ApiController]
public class IdentityController : ControllerBase
{
    private readonly IMediator _mediator;

    public IdentityController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("/api/auth/register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RegisterUserCommand
        {
            Email = request.Email,
            Password = request.Password,
            FirstName = request.FirstName,
            LastName = request.LastName
        };

        var result = await _mediator.Send(command, cancellationToken);
        
        return result.IsSuccess ? Ok(new { token = result.Value }) : result.ToProblem();
    }
    
    [HttpGet("/api/auth/verify")]
    public async Task<IActionResult> Verify([FromQuery] VerifyUserRequest request,
        CancellationToken cancellationToken)
    {
        var command = new VerifyUserCommand
        {
            Id = request.UserId,
            Token = request.Token
        };

        var result = await _mediator.Send(command, cancellationToken);
        
        return result.IsSuccess ? Ok() : result.ToProblem();
    }

    [HttpPost("/api/auth/login")]
    public async Task<IActionResult> Login([FromBody] LoginUserRequest request,
        CancellationToken cancellationToken)
    {
        var command = new LoginUserCommand
        {
            Email = request.Email,
            Password = request.Password
        };
        
        var result = await _mediator.Send(command, cancellationToken);
        
        return result.IsSuccess ? Ok(new { token = result.Value }) : result.ToProblem();
    }

    [HttpPost("/api/auth/logout")]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        var command = new LogoutUserCommand();
        
        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    
    [Authorize(Policy = "RequireLogin")]
    [HttpGet("/api/users/{id}")]
    public async Task<IActionResult> GetUser([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(id);
        
        var result = await _mediator.Send(query, cancellationToken);
        
        return result.IsSuccess ? Ok(result.Value.ToResponse()) : result.ToProblem();
    }
}