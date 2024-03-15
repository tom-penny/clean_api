namespace Shop.Application.Products.Commands;

public class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty();
        
        RuleFor(c => c.Stock)
            .GreaterThanOrEqualTo(0);
        
        RuleFor(c => c.Price)
            .GreaterThan(0.0m);
    }
}