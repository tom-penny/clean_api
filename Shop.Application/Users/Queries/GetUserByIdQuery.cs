namespace Shop.Application.Users.Queries;

using Common.Interfaces;
using Domain.Entities;

public record GetUserByIdQuery(Guid UserId) : IAuthorizedRequest<Result<User>>;