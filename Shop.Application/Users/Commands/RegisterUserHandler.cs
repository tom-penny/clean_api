using System.Transactions;

namespace Shop.Application.Users.Commands;

using Common.Interfaces;
using Domain.Entities;
using Domain.Errors;

public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, Result<string>>
{
    private readonly IApplicationDbContext _context;
    private readonly IUserService _userService;
    private readonly IJwtProvider _jwtProvider;
    
    public RegisterUserHandler(IApplicationDbContext context, IUserService userService, IJwtProvider jwtProvider)
    {
        _context = context;
        _userService = userService;
        _jwtProvider = jwtProvider;
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
        
        var loginResult = await _userService.SignInUserAsync(request.Email, request.Password);

        if (loginResult.IsFailed) return Result.Fail(UserError.InvalidCredentials());
        
        var roleResult = await _userService.GetRolesByIdAsync(loginResult.Value);
        
        var token = _jwtProvider.GenerateToken(loginResult.Value, request.Email, roleResult.Value);

        return Result.Ok(token);
    }
}