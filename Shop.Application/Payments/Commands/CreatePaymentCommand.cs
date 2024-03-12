namespace Shop.Application.Payments.Commands;

using Domain.Entities;

public class CreatePaymentCommand : IRequest<Result<Payment>>
{
    public required string CheckoutId { get; init; }
    public required string Status { get; init; }
    public required decimal Amount { get; init; }
}