namespace Shop.Application.Orders.Commands;

public class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderValidator()
    {
        RuleFor(c => c.CheckoutId)
            .NotEmpty();

        RuleFor(c => c.UserId)
            .NotEmpty();

        RuleFor(c => c.Amount)
            .GreaterThan(0.0m);
        
        RuleFor(c => c.Items)
            .NotEmpty();

        RuleForEach(c => c.Items)
            .ChildRules(item =>
            {
                item.RuleFor(i => i.ProductId)
                    .NotEmpty();
                item.RuleFor(i => i.Quantity)
                    .GreaterThan(0);
                item.RuleFor(i => i.UnitPrice)
                    .GreaterThan(0.0m);
            });
    }
}