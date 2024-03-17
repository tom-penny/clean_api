using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Shop.API.Controllers;

using Mappings;
using Extensions;
using Models.Requests;
using Application.Categories.Commands;
using Application.Categories.Queries;

[ApiController]
public class CategoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoryController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("/api/categories/{id}")]
    public async Task<IActionResult> GetCategory([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query = new GetCategoryByIdQuery { Id = id };
        
        var result = await _mediator.Send(query, cancellationToken);
        
        return result.IsSuccess ? Ok(result.Value.ToResponse()) : result.ToProblem();
    }
    
    [HttpGet("/api/categories")]
    public async Task<IActionResult> GetAllCategories(CancellationToken cancellationToken)
    {
        var query = new GetAllCategoriesQuery();
        
        var result = await _mediator.Send(query, cancellationToken);
        
        var categories = result.Value.Select(o => o.ToResponse()).ToList();
        
        return Ok(new { categories });
    }
    
    [Authorize(Policy = "RequireAdmin")]
    [HttpPost("/api/categories")]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateCategoryCommand(request.Name);

        var result = await _mediator.Send(command, cancellationToken);
        
        if (result.IsFailed) return result.ToProblem();
        
        var category = result.Value.ToResponse();

        return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
    }
    
    [Authorize(Policy = "RequireAdmin")]
    [HttpPut("/api/categories/{id}")]
    public async Task<IActionResult> UpdateCategory([FromRoute] Guid id,
        [FromBody] UpdateCategoryRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateCategoryCommand
        {
            Id = id,
            Name = request.Name
        };

        var result = await _mediator.Send(command, cancellationToken);
        
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    
    [Authorize(Policy = "RequireAdmin")]
    [HttpDelete("/api/categories/{id}")]
    public async Task<IActionResult> DeleteCategory([FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteCategoryCommand(id);

        var result = await _mediator.Send(command, cancellationToken);
        
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}