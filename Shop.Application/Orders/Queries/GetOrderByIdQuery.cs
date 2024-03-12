namespace Shop.Application.Orders.Queries;

using Interfaces;
using Domain.Entities;

public record GetOrderByIdQuery(Guid Id, Guid UserId) : IAuthorizedRequest<Result<Order>>;