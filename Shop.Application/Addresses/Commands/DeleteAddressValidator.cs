namespace Shop.Application.Addresses.Commands;

public class DeleteAddressValidator : AbstractValidator<DeleteAddressCommand>
{
    public DeleteAddressValidator()
    {
        RuleFor(c => c.Id)
            .NotEqual(Guid.Empty);

        RuleFor(c => c.UserId)
            .NotEqual(Guid.Empty);
    }
}