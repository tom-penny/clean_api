namespace Shop.Application.Users.Commands;

public class LoginUserValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserValidator()
    {
        RuleFor(c => c.Email)
            .EmailAddress();

        RuleFor(c => c.Password)
            .NotEmpty();
    }
}