namespace Shop.Application.Products.Queries;

public class GetProductsByCategoryValidator : AbstractValidator<GetProductsByCategoryQuery>
{
    private static readonly string[] SortValues = { "name", "price", "date" };
    private static readonly string[] OrderValues = { "asc", "desc" };
    
    public GetProductsByCategoryValidator()
    {
        RuleFor(q => q.CategoryIdOrSlug)
            .NotEmpty();
        
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