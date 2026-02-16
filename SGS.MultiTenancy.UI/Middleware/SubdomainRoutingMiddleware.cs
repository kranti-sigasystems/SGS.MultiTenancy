using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SGS.MultiTenancy.Core.Application.TenantContext;
using SGS.MultiTenancy.Core.Domain.Entities.Auth;
using SGS.MultiTenancy.Core.Domain.Enums;
using SGS.MultiTenancy.Infra.DataContext;

public class SubdomainRoutingMiddleware
{
    //private readonly RequestDelegate _next;

    //public SubdomainRoutingMiddleware(RequestDelegate next)
    //{
    //    _next = next;
    //}

    //public async Task InvokeAsync(HttpContext context)
    //{
    //    string path = context.Request.Path.Value?.TrimEnd('/').ToLower() ?? "/";

    //    if (path.StartsWith("/css") ||
    //        path.StartsWith("/js") ||
    //        path.StartsWith("/images") ||
    //        path.StartsWith("/favicon.ico"))
    //    {
    //        await _next(context);
    //        return;
    //    }

    //    if (path.StartsWith("/auth/login"))
    //    {
    //        await _next(context);
    //        return;
    //    }

    //    if (context.User?.Identity?.IsAuthenticated != true)
    //    {
    //        // Do NOT interrupt POST login processing
    //        if (context.Request.Method == HttpMethods.Post)
    //        {
    //            await _next(context);
    //            return;
    //        }

    //        context.Response.Redirect("/Auth/Login");
    //        return;
    //    }


    //    string host = context.Request.Host.Host.ToLower();

    //    // ===== IMPORTANT FIX =====
    //    // Bypass for localhost or non-subdomain environments
    //    if (host == "localhost" || host == "127.0.0.1")
    //    {
    //        await _next(context);
    //        return;
    //    }

    //    string[] hostParts = host.Split('.');

    //    // No subdomain → not tenant portal
    //    if (hostParts.Length < 3)
    //    {
    //        await _next(context);
    //        return;
    //    }

    //    string subdomain = hostParts[0];
    //    using IServiceScope scope = context.RequestServices.CreateScope();
    //    AppDbContext db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    //    // ===== SYSTEM ADMIN =====
    //    if (context.User?.Identity?.IsAuthenticated != true)
    //    {
    //        // Allow POST login to execute
    //        if (context.Request.Method == HttpMethods.Post)
    //        {
    //            await _next(context);
    //            return;
    //        }

    //        context.Response.Redirect("/Auth/Login");
    //        return;
    //    }

    //    // ===== INVALID HOST =====
    //    if (subdomain == "www" || string.IsNullOrWhiteSpace(subdomain))
    //    {
    //        context.Response.Redirect("/Auth/Login");
    //        return;
    //    }

    //    // ===== TENANT PORTAL =====
    //    context.Items["Portal"] = "Tenant";

    //    Tenant? tenant = await db.Tenants.FirstOrDefaultAsync(
    //        t => t.Slug == subdomain && t.Status == EntityStatus.Active);

    //    if (tenant == null)
    //    {
    //        context.Response.StatusCode = 404;
    //        await context.Response.WriteAsync("Tenant not found");
    //        return;
    //    }

    //    context.Items["Tenant"] = tenant;

    //    if (path == "/" && context.User?.Identity?.IsAuthenticated != true)
    //    {
    //        context.Response.Redirect("/Home/Index");
    //        return;
    //    }

    //    await _next(context);
    //}

    private readonly RequestDelegate _next;

    public SubdomainRoutingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync( HttpContext context,  AppDbContext dbContext, TenantContext tenantContext)
    {
        var port = context.Request.Host.Port;
        var host = context.Request.Host.Host;
        var segments = host.Split('.');

        // Host domain: localhost
        if (segments.Length < 2)
        {
            tenantContext.IsHost = true;
            await _next(context);
            return;
        }

        // Tenant domain
        var slug = segments[0];

        var tenant = await dbContext.Tenants
            .Where(x => x.Slug == slug)
            .Select(x => new { x.ID, x.Slug })
            .FirstOrDefaultAsync(context.RequestAborted);

        if (tenant is null)
        {
            context.Response.Redirect($"{context.Request.Scheme}://localhost:{port}/Auth/Login");
            return;
        }

        tenantContext.IsHost = false;
        tenantContext.TenantId = tenant.ID;
        tenantContext.TenantSlug = tenant.Slug;

        await _next(context);
    }
}
