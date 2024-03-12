namespace Shop.Application.Categories.Queries;

public class GetCategoryByIdValidator : AbstractValidator<GetCategoryByIdQuery>
{
    public GetCategoryByIdValidator()
    {
        RuleFor(q => q.Id)
            .NotEqual(Guid.Empty);
    }
}