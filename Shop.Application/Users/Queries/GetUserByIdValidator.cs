namespace Shop.Application.Users.Queries;

public class GetUserByIdValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdValidator()
    {
        RuleFor(q => q.UserId)
            .NotEqual(Guid.Empty);
    }
}