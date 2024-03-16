namespace Shop.Application.Common.Interfaces;

public interface IJwtProvider
{
    string GenerateToken(Guid userId, string email, List<string> roles);
}