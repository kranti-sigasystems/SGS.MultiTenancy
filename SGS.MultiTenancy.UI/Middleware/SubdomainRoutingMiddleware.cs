using Microsoft.EntityFrameworkCore;
using SGS.MultiTenancy.Core.Domain.Entities.Auth;
using SGS.MultiTenancy.Core.Domain.Enums;
using SGS.MultiTenancy.Infra.DataContext;
public class SubdomainRoutingMiddleware
{
    private readonly RequestDelegate _next;

    public SubdomainRoutingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        
        string path = context.Request.Path.Value?.TrimEnd('/').ToLower() ?? "/";

        if (path.StartsWith("/css") || path.StartsWith("/js") || path.StartsWith("/images") || path.StartsWith("/favicon.ico"))
        {
            await _next(context);
            return;
        }

        if (path.StartsWith("/auth/login") || path.StartsWith("/auth/register"))
        {
            await _next(context);
            return;
        }

        if (context.User?.Identity?.IsAuthenticated == true)
        {
            await _next(context);
            return;
        }

        string host = context.Request.Host.Host.ToLower();
        string subdomain = host.Split('.')[0];

        using IServiceScope scope = context.RequestServices.CreateScope();
        AppDbContext db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (subdomain == "sgs")
        {
            context.Items["Portal"] = "SystemAdmin";

            if (path == "/" && context.User?.Identity?.IsAuthenticated != true)
            {
                context.Response.Redirect("/Auth/Login");
                return;
            }
        }
        else
        {
            context.Items["Portal"] = "Tenant";

            Tenant? tenant = await db.Tenants.FirstOrDefaultAsync(
                t => t.Slug == subdomain && t.Status == EntityStatus.Active);

            if (tenant == null)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync("Tenant not found");
                return;
            }

            context.Items["Tenant"] = tenant;

            if (path == "/" && context.User?.Identity?.IsAuthenticated != true)
            {
                context.Response.Redirect("/Home/Index");
                return;
            }
        }

        await _next(context);
    }
}
