namespace Shop.Application.Payments.Queries;

using Domain.Entities;

public record GetPaymentByIdQuery(Guid Id) : IRequest<Result<Payment>>;