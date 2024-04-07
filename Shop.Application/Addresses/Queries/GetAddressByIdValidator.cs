namespace Shop.Application.Addresses.Queries;

public class GetAddressByIdValidator : AbstractValidator<GetAddressByIdQuery>
{
    public GetAddressByIdValidator()
    {
        RuleFor(q => q.Id)
            .NotEqual(Guid.Empty);

        RuleFor(q => q.UserId)
            .NotEqual(Guid.Empty);
    }
}