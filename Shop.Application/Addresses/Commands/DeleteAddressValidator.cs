namespace Shop.Application.Addresses.Commands;

public class DeleteAddressValidator : AbstractValidator<DeleteAddressCommand>
{
    public DeleteAddressValidator()
    {
        RuleFor(c => c.Id)
            .NotEqual(Guid.Empty);
    }
}