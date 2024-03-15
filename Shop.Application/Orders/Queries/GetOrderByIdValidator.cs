namespace Shop.Application.Orders.Queries;

public class GetOrderByIdValidator : AbstractValidator<GetOrderByIdQuery>
{
    public GetOrderByIdValidator()
    {
        RuleFor(q => q.Id)
            .NotEqual(Guid.Empty);

        RuleFor(q => q.UserId)
            .NotEqual(Guid.Empty);
    }
}