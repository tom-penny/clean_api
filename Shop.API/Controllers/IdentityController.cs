using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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

        if (!result.IsSuccess) return result.ToProblem();

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(7)
        };
        
        Response.Cookies.Append("token", result.Value, cookieOptions);

        return Ok();
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
        
        if (!result.IsSuccess) return result.ToProblem();

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(7)
        };
        
        Response.Cookies.Append("token", result.Value, cookieOptions);

        return Ok();
    }

    [HttpPost("/api/auth/logout")]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        var command = new LogoutUserCommand();
        
        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }

    [Authorize(Policy = "RequireLogin")]
    [HttpGet("/api/auth/me")]
    public async Task<IActionResult> GetCurrentUser(CancellationToken cancellationToken)
    {
        var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var userId = Guid.TryParse(claim, out var guid) ? guid : Guid.Empty;

        var query = new GetUserByIdQuery(userId);
        
        var result = await _mediator.Send(query, cancellationToken);
        
        return result.IsSuccess ? Ok(result.Value.ToResponse()) : result.ToProblem();
    }
    
    [Authorize(Policy = "RequireLogin")]
    [HttpGet("/api/users/{id}")]
    public async Task<IActionResult> GetUser([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(id);
        
        var result = await _mediator.Send(query, cancellationToken);
        
        return result.IsSuccess ? Ok(result.Value.ToResponse()) : result.ToProblem();
    }
    
    [Authorize(Policy = "RequireAdmin")]
    [HttpGet("/api/users")]
    public async Task<IActionResult> GetAllUsers([FromQuery] GetAllUsersRequest request,
        CancellationToken cancellationToken)
    {
        var query = new GetAllUsersQuery
        {
            SortBy = request.Sort,
            OrderBy = request.Order,
            Page = request.Page,
            Size = request.Size
        };
        
        var result = await _mediator.Send(query, cancellationToken);

        var response = result.Value.ToResponse();
        
        return Ok(response);
    }
}