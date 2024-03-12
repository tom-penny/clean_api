namespace Shop.API.Models.Requests;

public class CreatePaymentRequest
{
    public required string CheckoutId { get; init; }
    public required string Status { get; init; }
    public required decimal Amount { get; init; }
}