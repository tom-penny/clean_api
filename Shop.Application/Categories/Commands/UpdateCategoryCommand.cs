namespace Shop.Application.Categories.Commands;

public class UpdateCategoryCommand : IRequest<Result<Unit>>
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
}