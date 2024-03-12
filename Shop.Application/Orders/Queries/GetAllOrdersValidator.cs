namespace Shop.Application.Orders.Queries;

public class GetAllOrdersValidator : AbstractValidator<GetAllOrdersQuery>
{
    public GetAllOrdersValidator()
    {
        RuleFor(q => q.UserId)
            .NotEqual(Guid.Empty);
    }
}