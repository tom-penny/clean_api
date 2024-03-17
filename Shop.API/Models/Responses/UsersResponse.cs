namespace Shop.API.Models.Responses;

public class UsersResponse : PagedResponse
{
    public required List<UserResponse> Users { get; init; }
}