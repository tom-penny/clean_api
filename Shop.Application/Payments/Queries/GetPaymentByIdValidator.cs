namespace Shop.Application.Payments.Queries;

public class GetPaymentByIdValidator : AbstractValidator<GetPaymentByIdQuery>
{
    public GetPaymentByIdValidator()
    {
        RuleFor(q => q.Id)
            .NotEqual(Guid.Empty);
    }
}