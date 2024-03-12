namespace Shop.API.Models.Requests;

public class VerifyUserRequest
{
    public required Guid UserId { get; init; }
    public required string Token { get; init; }
}