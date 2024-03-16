namespace Shop.Application.Orders.Queries;

using Common.Interfaces;
using Domain.Entities;

public record GetOrderByIdQuery(Guid Id, Guid UserId) : IAuthorizedRequest<Result<Order>>;