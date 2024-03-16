namespace Shop.Application.Common.Interfaces;

public interface IResourceAuthorizationService
{
    Task<bool> AuthorizeAccessAsync(Guid userId, string policyName);
}