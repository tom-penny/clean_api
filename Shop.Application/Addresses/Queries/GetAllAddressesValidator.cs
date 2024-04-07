namespace Shop.Application.Addresses.Queries;

public class GetAllAddressesValidator : AbstractValidator<GetAllAddressesQuery>
{
    public GetAllAddressesValidator()
    {
        RuleFor(q => q.UserId)
            .NotEqual(Guid.Empty);
    }
}