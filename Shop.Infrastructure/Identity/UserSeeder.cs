using Microsoft.AspNetCore.Identity;

namespace Shop.Infrastructure.Identity;

public class UserSeeder
{
    private readonly UserManager<IdentityUser<Guid>> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public UserSeeder(UserManager<IdentityUser<Guid>> userManager,
        RoleManager<IdentityRole<Guid>> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task SeedAsync()
    {
        const string role = "Admin";
        const string email = "admin@test.com";
        const string password = "Abc123!";
        
        if (!await _roleManager.RoleExistsAsync(role))
        {
            await _roleManager.CreateAsync(new IdentityRole<Guid> { Name = role });
        }
        
        var user = await _userManager.FindByEmailAsync(email);
        
        if (user != null) return;

        user = new IdentityUser<Guid>
        {
            Id = Guid.NewGuid(),
            UserName = email,
            Email = email
        };

        await _userManager.CreateAsync(user, password);

        await _userManager.AddToRoleAsync(user, role);
    }
}