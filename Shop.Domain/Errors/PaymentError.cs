namespace Shop.Domain.Errors;

using Primitives;

public static class PaymentError
{
    public static DomainError NotFound(Guid id) =>
        DomainError.NotFound($"Payment {id} not found");
}