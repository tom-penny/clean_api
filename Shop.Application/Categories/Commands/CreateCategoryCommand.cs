namespace Shop.Application.Categories.Commands;

using Domain.Entities;

public record CreateCategoryCommand(string Name) : IRequest<Result<Category>>;