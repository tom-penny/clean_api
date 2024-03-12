namespace Shop.Application.Categories.Commands;

public class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty();
    }
}