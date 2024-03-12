namespace Shop.Application.Orders.Queries;

using Interfaces;
using Domain.Entities;

public record GetAllOrdersQuery(Guid UserId) : IAuthorizedRequest<Result<List<Order>>>;