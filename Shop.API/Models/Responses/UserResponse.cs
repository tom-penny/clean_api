namespace Shop.API.Models.Responses;

public class UserResponse
{
    public required string Id { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required DateTime Joined { get; init; }
}