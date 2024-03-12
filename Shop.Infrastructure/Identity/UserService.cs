using Microsoft.AspNetCore.Identity;

namespace Shop.Infrastructure.Identity;

using Application.Interfaces;

public class UserService : IUserService
{
    private readonly UserManager<IdentityUser<Guid>> _userManager;
    private readonly SignInManager<IdentityUser<Guid>> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserService(UserManager<IdentityUser<Guid>> userManager,
        SignInManager<IdentityUser<Guid>> signInManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    public async Task<Result<Guid>> CreateUserAsync(string email, string password)
    {
        var userId = Guid.NewGuid();
        
        var user = new IdentityUser<Guid>
        {
            Id = userId,
            UserName = email,
            Email = email
        };

        var result = await _userManager.CreateAsync(user, password);

        return result.Succeeded ? Result.Ok(userId) : Result.Fail("Registration failed");
    }

    public async Task<Result<Guid>> SignInUserAsync(string email, string password)
    {
        var result = await _signInManager.PasswordSignInAsync(email, password, false, false);

        if (!result.Succeeded) return Result.Fail("Invalid credentials");
        
        var user = await _userManager.FindByEmailAsync(email);

        return user?.Id != null ? Result.Ok(user.Id) : Result.Fail("User not found");
    }
    
    public async Task<Result> SignOutUserAsync()
    {
        await _signInManager.SignOutAsync();

        return Result.Ok();
    }

    public async Task<Result<string>> GenerateConfirmationTokenAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user == null) return Result.Fail("User not found");

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        return Result.Ok(token);
    }

    public async Task<Result> ValidateConfirmationTokenAsync(Guid userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user == null) return Result.Fail("User not found");

        var result = await _userManager.ConfirmEmailAsync(user, token);
        
        return Result.OkIf(result.Succeeded, "Email validation failed");
    }

    public async Task<Result<List<string>>> GetRolesByIdAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user == null) return Result.Fail("User not found");

        var roles = await _userManager.GetRolesAsync(user);

        return Result.Ok(roles.ToList());
    }

    public async Task<Result> AddRoleAsync(Guid userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user == null) return Result.Fail("User not found");

        if (!await _roleManager.RoleExistsAsync(role))
        {
            return Result.Fail("Role not found");
        }
        
        var result = await _userManager.AddToRoleAsync(user, role);
            
        return Result.OkIf(result.Succeeded, "Role assignment failed");
    }

    public async Task<Result> RemoveRoleAsync(Guid userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user == null) return Result.Fail("User not found");

        var result = await _userManager.RemoveFromRoleAsync(user, role);
        
        return Result.OkIf(result.Succeeded, "Role withdrawal failed");
    }
}