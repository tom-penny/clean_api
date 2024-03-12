namespace Shop.Application.Users.Queries;

using Interfaces;
using Domain.Entities;

public record GetUserByIdQuery(Guid UserId) : IAuthorizedRequest<Result<User>>;