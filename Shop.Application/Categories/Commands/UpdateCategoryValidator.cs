namespace Shop.Application.Categories.Commands;

public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryValidator()
    {
        RuleFor(c => c.Id)
            .NotEqual(Guid.Empty);
        
        RuleFor(c => c.Name)
            .NotEmpty();
    }
}