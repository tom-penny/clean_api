namespace Shop.Application.Common.Behaviors;

using Interfaces;
using Domain.Primitives;

public class AuthorizationBehavior<TRequest, TResponse> :
    IPipelineBehavior<TRequest, TResponse>
    where TRequest : IAuthorizedRequest<TResponse>
    where TResponse : class, IResultBase
{
    private readonly IResourceAuthorizationService _authorizationService;

    public AuthorizationBehavior(IResourceAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }
    
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var authorized = await _authorizationService.AuthorizeAccessAsync(request.UserId, "ResourceAccess");
        
        if (authorized) return await next();
        
        var result = Result.Fail(DomainError.Forbidden("No permission to access requested resource.")) as TResponse;
        
        return result ?? throw new UnauthorizedAccessException();
    }
}