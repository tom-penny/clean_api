namespace Shop.Application.Products.Queries;

public class GetProductByIdValidator : AbstractValidator<GetProductByIdQuery>
{
    public GetProductByIdValidator()
    {
        RuleFor(q => q.Id)
            .NotEqual(Guid.Empty);
    }
}