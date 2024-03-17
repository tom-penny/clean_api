namespace Shop.Application.Orders.Queries;

public class GetAllOrdersValidator : AbstractValidator<GetAllOrdersQuery>
{
    private static readonly string[] SortValues = { "total", "date" };
    private static readonly string[] OrderValues = { "asc", "desc" };
    
    public GetAllOrdersValidator()
    {
        RuleFor(q => q.UserId)
            .NotEqual(Guid.Empty);
        
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