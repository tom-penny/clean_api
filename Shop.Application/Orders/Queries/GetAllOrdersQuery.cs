namespace Shop.Application.Orders.Queries;

using Common.Interfaces;
using Domain.Entities;

public record GetAllOrdersQuery(Guid UserId) : IAuthorizedRequest<Result<List<Order>>>;