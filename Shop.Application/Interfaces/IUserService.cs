namespace Shop.Application.Interfaces;

public interface IUserService
{
    Task<Result<Guid>> CreateUserAsync(string email, string password);
    Task<Result<Guid>> SignInUserAsync(string email, string password);
    Task<Result> SignOutUserAsync();
    Task<Result<string>> GenerateConfirmationTokenAsync(Guid userId);
    Task<Result> ValidateConfirmationTokenAsync(Guid userId, string token);
    Task<Result<List<string>>> GetRolesByIdAsync(Guid userId);
    Task<Result> AddRoleAsync(Guid userId, string role);
    Task<Result> RemoveRoleAsync(Guid userId, string role);
}