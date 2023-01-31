using IdentityModel;
using IdentityServer.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace IdentityServer.Utils
{
    public class AuthorizationRequirement : IAuthorizationRequirement
    {
        public AuthorizationRequirement(string permissionName)
        {
            PermissionName = permissionName;
        }

        public string PermissionName { get; }
    }


    public class PermissionHandler : AuthorizationHandler<AuthorizationRequirement>
    {
        private readonly IAccesService _accesService;
        public PermissionHandler(IAccesService accesService)
        {
            _accesService = accesService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizationRequirement requirement)
        {
            if (context.Resource is HttpContext httpContext && httpContext.GetEndpoint() is RouteEndpoint endpoint)
            {
                endpoint.RoutePattern.RequiredValues.TryGetValue("controller", out var _controller);
                endpoint.RoutePattern.RequiredValues.TryGetValue("action", out var _action);

                if(!string.IsNullOrWhiteSpace(requirement?.PermissionName)&& !requirement.PermissionName.Equals("Authorization"))
                {
                    _action = requirement.PermissionName;
                }
                if (requirement != null && context.User.Identity?.IsAuthenticated == true && _controller != null && _action != null 
                    && await _accesService.GetMenuItems(context.User,_controller.ToString(),_action.ToString()))
                {
                    
                    context.Succeed(requirement);
                }
                await Task.CompletedTask;

            }
        }
    }
}
