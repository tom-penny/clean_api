namespace Shop.Application.Users.Commands;

public class VerifyUserValidator : AbstractValidator<VerifyUserCommand>
{
    public VerifyUserValidator()
    {
        RuleFor(c => c.Id)
            .NotEqual(Guid.Empty);

        RuleFor(c => c.Token)
            .NotEmpty();
    }
}