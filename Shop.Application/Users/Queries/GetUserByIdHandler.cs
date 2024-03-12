namespace Shop.Application.Users.Queries;

using Domain.Errors;
using Domain.Entities;

public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, Result<Shop.Domain.Entities.User>>
{
    private readonly Shop.Application.Interfaces.IApplicationDbContext _context;

    public GetUserByIdHandler(Shop.Application.Interfaces.IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Shop.Domain.Entities.User>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u =>
            u.Id == new UserId(request.UserId), cancellationToken);
            
        return user != null ? Result.Ok(user) : Result.Fail(UserError.NotFound(request.UserId));
    }
}