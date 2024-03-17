namespace Shop.Application.Users.Queries;

public class GetAllUsersValidator : AbstractValidator<GetAllUsersQuery>
{
    private static readonly string[] SortValues = { "email", "name", "date" };
    private static readonly string[] OrderValues = { "asc", "desc" };
    
    public GetAllUsersValidator()
    {
        RuleFor(q => q.SortBy)
            .Must(v => v == null || SortValues.Contains(v, StringComparer.OrdinalIgnoreCase));
        
        RuleFor(q => q.OrderBy)
            .Must(v => v == null || OrderValues.Contains(v, StringComparer.OrdinalIgnoreCase));

        RuleFor(q => q.Page)
            .GreaterThanOrEqualTo(1);

        RuleFor(q => q.Size)
            .InclusiveBetween(1, 25);
    }
}