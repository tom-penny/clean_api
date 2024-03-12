namespace Shop.Application.Interfaces;

public interface IResourceAuthorizationService
{
    Task<bool> AuthorizeAccessAsync(Guid userId, string policyName);
}