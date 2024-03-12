namespace Shop.API.Mappings;

using Domain.Entities;
using Models.Responses;

public static class UserMapping
{
    public static UserResponse ToResponse(this User user)
    {
        return new UserResponse
        {
            Id = user.Id.Value.ToString(),
            FirstName = user.FirstName,
            LastName = user.LastName,
            Joined = user.Joined
        };
    }
}