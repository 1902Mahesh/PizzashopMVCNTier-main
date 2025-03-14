using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

public class PermissionAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly string _requiredPermission;

    public PermissionAuthorizeAttribute(string requiredPermission)
    {
        _requiredPermission = requiredPermission;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;
        
        if (user == null || !user.Identity.IsAuthenticated)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var hasPermission = user.Claims.Any(c => c.Type == "permission" && c.Value == _requiredPermission);

        if (!hasPermission)
        {
            context.Result = new ForbidResult();
        }
    }
}
