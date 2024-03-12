namespace Shop.API.Models.Responses;

public class PaymentResponse
{
    public required string Id { get; init; }
    public required string CheckoutId { get; init; }
    public required decimal Amount { get; init; }
    public required string Status { get; init; }
}