namespace Shop.Application.Categories.Commands;

public record DeleteCategoryCommand(Guid Id) : IRequest<Result<Unit>>;