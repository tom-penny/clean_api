namespace Shop.Application.Users.Queries;

using Common.Interfaces;
using Domain.Errors;
using Domain.Entities;

public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, Result<User>>
{
    private readonly IApplicationDbContext _context;

    public GetUserByIdHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<User>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u =>
            u.Id == new UserId(request.UserId), cancellationToken);
            
        return user != null ? Result.Ok(user) : Result.Fail(UserError.NotFound(request.UserId));
    }
}