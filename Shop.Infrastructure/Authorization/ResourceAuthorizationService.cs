using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace Shop.Infrastructure.Authorization;

using Application.Common.Interfaces;

public class ResourceAuthorizationService : IResourceAuthorizationService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAuthorizationService _authorizationService;

    public ResourceAuthorizationService(IHttpContextAccessor httpContextAccessor, IAuthorizationService authorizationService)
    {
        _httpContextAccessor = httpContextAccessor;
        _authorizationService = authorizationService;
    }

    public async Task<bool> AuthorizeAccessAsync(Guid userId, string policyName)
    {
        var user = _httpContextAccessor.HttpContext?.User;

        if (user == null) return false;

        var authorized = await _authorizationService.AuthorizeAsync(user, userId, policyName);

        return authorized.Succeeded;
    }
}