namespace Shop.Application.Addresses.Commands;

using Common.Interfaces;
using Domain.Entities;
using Domain.Errors;

public class CreateAddressHandler : IRequestHandler<CreateAddressCommand, Result<Address>>
{
    private readonly IApplicationDbContext _context;

    public CreateAddressHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Address>> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
    {
        var userExists = await _context.Users.AnyAsync(u =>
            u.Id == new UserId(request.UserId), cancellationToken);

        if (!userExists) return Result.Fail(AddressError.UserNotFound(request.UserId));

        var address = new Address
        (
            id: new AddressId(Guid.NewGuid()),
            userId: new UserId(request.UserId),
            street: request.Street,
            city: request.City,
            country: request.Country,
            postCode: request.PostCode
        );

        _context.Addresses.Add(address);
        
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Ok(address);
    }
}