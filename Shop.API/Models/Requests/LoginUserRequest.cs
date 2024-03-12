namespace Shop.API.Models.Requests;

public class LoginUserRequest
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}