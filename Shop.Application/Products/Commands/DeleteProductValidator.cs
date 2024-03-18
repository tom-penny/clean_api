namespace Shop.Application.Products.Commands;

public class DeleteProductValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductValidator()
    {
        RuleFor(c => c.Id)
            .NotEqual(Guid.Empty);
    }
}