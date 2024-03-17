namespace Shop.API.Mappings;

using Models.Responses;
using Domain.Entities;
using Application.Common.Models;

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
    
    public static UsersResponse ToResponse<T>(this PagedList<T> list) where T : User
    {
        return new UsersResponse
        {
            Page = list.Page,
            Size = list.Size,
            Count = list.Count,
            Users = list.Items.Select(u => u.ToResponse()).ToList(),
        };
    }
}