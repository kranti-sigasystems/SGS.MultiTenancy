using System.Net;
using System.Text.Json;
using SGS.MultiTenancy.Core.Application.Interfaces;
using SGS.MultiTenancy.Core.Domain.Common;
using SGS.MultiTenancy.Infra.DataContext;

namespace SGS.MultiTenancy.UI.Middleware
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

        public GlobalExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception caught by global middleware");

                try
                {
                    ITenantProvider? tenantProvider = context.RequestServices.GetService(typeof(ITenantProvider)) as ITenantProvider;
                    AppDbContext? db = context.RequestServices.GetService(typeof(AppDbContext)) as AppDbContext;

                    string? userInfo = null;
                    if (context.User?.Identity?.IsAuthenticated == true)
                    {
                        string? name = context.User.Identity.Name;
                        string? id = context.User.FindFirst("sub")?.Value
                                 ?? context.User.FindFirst("id")?.Value
                                 ?? context.User.FindFirst("userid")?.Value;

                        userInfo = string.IsNullOrEmpty(name)
                            ? id
                            : string.IsNullOrEmpty(id) ? name : $"{name} ({id})";
                    }

                    MultiTenantAppStatusLog log = new ()
                    {
                        LogId = Guid.NewGuid(),
                        LogLevel = SGS.MultiTenancy.Core.Domain.Enums.LogLevel.Error,
                        TimeStamp = DateTime.UtcNow,
                        TenantId = tenantProvider?.TenantId == Guid.Empty ? null : tenantProvider?.TenantId,
                        Message = ex.ToString(),
                        HttpStatusCode = StatusCodes.Status500InternalServerError,
                        UserInfo = userInfo
                    };

                    if (db != null)
                    {
                        db.MultiTenantAppStatusLogs.Add(log);
                        await db.SaveChangesAsync();
                    }
                }
                catch (Exception logEx)
                {
                    _logger.LogError(logEx, "Failed to write exception log to database");
                }

                if (context.Response.HasStarted)
                {
                    _logger.LogWarning("Response has already started, rethrowing exception");
                    throw;
                }

                context.Response.Clear();
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                IWebHostEnvironment? env = context.RequestServices.GetService(
                    typeof(Microsoft.AspNetCore.Hosting.IWebHostEnvironment)
                ) as Microsoft.AspNetCore.Hosting.IWebHostEnvironment;

                string? response = JsonSerializer.Serialize(new
                {
                    error = "An unexpected error occurred.",
                    traceId = context.TraceIdentifier,
                    detail = env?.IsDevelopment() == true ? ex.Message : null
                });

                await context.Response.WriteAsync(response);
            }
        }
    }

    public static class GlobalExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder app)
        {
            return app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
        }
    }
}
