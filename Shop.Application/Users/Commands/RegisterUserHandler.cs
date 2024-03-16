using System.Transactions;

namespace Shop.Application.Users.Commands;

using Common.Interfaces;
using Domain.Entities;
using Domain.Errors;

public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, Result<string>>
{
    private readonly IApplicationDbContext _context;
    private readonly IUserService _userService;
    
    public RegisterUserHandler( IApplicationDbContext context, IUserService userService)
    {
        _userService = userService;
        _context = context;
    }

    public async Task<Result<string>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var emailExists = await _context.Users.AnyAsync(u =>
            u.Email == request.Email, cancellationToken);

        if (emailExists) return Result.Fail(UserError.EmailExists());

        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            var result = await _userService.CreateUserAsync(request.Email, request.Password);
        
            if (result.IsFailed) return Result.Fail(UserError.RegistrationFailed());
        
            var user = new User
            (
                id: new UserId(result.Value),
                firstName: request.FirstName,
                lastName: request.LastName,
                email: request.Email
            );
        
            _context.Users.Add(user);
        
            await _context.SaveChangesAsync(cancellationToken);
        
            scope.Complete();
        }

        return Result.Ok();
    }
}