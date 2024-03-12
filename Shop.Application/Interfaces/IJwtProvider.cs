namespace Shop.Application.Interfaces;

public interface IJwtProvider
{
    string GenerateToken(Guid userId, string email, List<string> roles);
}