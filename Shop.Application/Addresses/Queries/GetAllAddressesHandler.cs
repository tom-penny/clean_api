namespace Shop.Application.Addresses.Queries;

using Common.Interfaces;
using Domain.Entities;

public class GetAllAddressesHandler : IRequestHandler<GetAllAddressesQuery, Result<List<Address>>>
{
    private readonly IApplicationDbContext _context;

    public GetAllAddressesHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<Address>>> Handle(GetAllAddressesQuery request, CancellationToken cancellationToken)
    {
        var addresses = await _context.Addresses.Where(a => a.UserId == new UserId(request.UserId))
            .ToListAsync(cancellationToken);
        
        return Result.Ok(addresses);
    }
}