namespace Shop.Application.Categories.Commands;

public class DeleteCategoryValidator : AbstractValidator<DeleteCategoryCommand>
{
    public DeleteCategoryValidator()
    {
        RuleFor(c => c.Id)
            .NotEqual(Guid.Empty);
    }
}