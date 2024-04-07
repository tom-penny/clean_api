namespace Shop.Application.Addresses.Commands;

using Common.Interfaces;
using Domain.Errors;
using Domain.Entities;

public class DeleteAddressHandler : IRequestHandler<DeleteAddressCommand, Result<Unit>>
{
    private readonly IApplicationDbContext _context;

    public DeleteAddressHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Unit>> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
    {
        var address = await _context.Addresses.FirstOrDefaultAsync(a =>
            a.Id == new AddressId(request.Id), cancellationToken);

        if (address == null) return Result.Fail(AddressError.NotFound(request.Id));
        
        address.Deactivate();
        
        await _context.SaveChangesAsync(cancellationToken);
        
        return Result.Ok();
    }
}