namespace Shop.Application.Users.Commands;

public class RegisterUserValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserValidator()
    {
        RuleFor(c => c.Email)
            .EmailAddress();

        RuleFor(c => c.Password)
            .NotEmpty();

        RuleFor(c => c.FirstName)
            .NotEmpty();
        
        RuleFor(c => c.LastName)
            .NotEmpty();
    }
}