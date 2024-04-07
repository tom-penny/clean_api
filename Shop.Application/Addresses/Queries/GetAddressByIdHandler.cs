namespace Shop.Application.Addresses.Queries;

using Common.Interfaces;
using Domain.Entities;
using Domain.Errors;

public class GetAddressByIdHandler : IRequestHandler<GetAddressByIdQuery, Result<Address>>
{
    private readonly IApplicationDbContext _context;

    public GetAddressByIdHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Result<Address>> Handle(GetAddressByIdQuery request, CancellationToken cancellationToken)
    {
        var address = await _context.Addresses.FirstOrDefaultAsync(a =>
            a.Id == new AddressId(request.Id), cancellationToken);
            
        return address != null ? Result.Ok(address) : Result.Fail(AddressError.NotFound(request.Id));
    }
}