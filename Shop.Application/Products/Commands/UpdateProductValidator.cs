namespace Shop.Application.Products.Commands;

public class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductValidator()
    {
        RuleFor(c => c.Id)
            .NotEqual(Guid.Empty);
        
        RuleFor(c => c.Name)
            .NotEmpty();
        
        RuleFor(c => c.Price)
            .GreaterThan(0.0m);
        
        RuleFor(c => c.CategoryIds)
            .NotEmpty();

        RuleForEach(c => c.CategoryIds)
            .NotEqual(Guid.Empty);
    }
}