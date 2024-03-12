namespace Shop.Application.Shipments.Commands;

public class UpdateShipmentValidator : AbstractValidator<UpdateShipmentCommand>
{
    public UpdateShipmentValidator()
    {
        RuleFor(c => c.Id)
            .NotEqual(Guid.Empty);
        
        RuleFor(c => c.DeliveryDate)
            .LessThanOrEqualTo(DateTime.UtcNow);
    }
}