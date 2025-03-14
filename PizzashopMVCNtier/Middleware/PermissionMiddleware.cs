using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

public class PermissionMiddleware
{
    private readonly RequestDelegate _next;

    public PermissionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var user = context.User;

        if (user.Identity.IsAuthenticated)
        {
            var permissions = user.Claims
                .Where(c => c.Type == "permission")
                .Select(c => c.Value)
                .ToList();

            context.Items["Permissions"] = permissions;
        }

        await _next(context);
    }
}
