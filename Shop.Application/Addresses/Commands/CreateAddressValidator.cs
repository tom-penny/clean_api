namespace Shop.Application.Addresses.Commands;

public class CreateAddressValidator : AbstractValidator<CreateAddressCommand>
{
    public CreateAddressValidator()
    {
        RuleFor(c => c.UserId)
            .NotEqual(Guid.Empty);
        
        RuleFor(c => c.Street)
            .NotEmpty();
        
        RuleFor(c => c.City)
            .NotEmpty();
        
        RuleFor(c => c.Country)
            .NotEmpty();
        
        RuleFor(c => c.PostCode)
            .NotEmpty();
    }
}