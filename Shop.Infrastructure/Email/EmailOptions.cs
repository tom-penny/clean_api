namespace Shop.Infrastructure.Email;

public class EmailOptions
{
    public required string Host { get; init; }
    public required int Port { get; init; }
    public required string Sender { get; init; }
    public required string Password { get; init; }
}