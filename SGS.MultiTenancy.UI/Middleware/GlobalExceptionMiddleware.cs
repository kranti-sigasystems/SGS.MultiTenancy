using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SGS.MultiTenancy.Core.Application.Interfaces;
using SGS.MultiTenancy.Core.Domain.Entities.Auth;
using SGS.MultiTenancy.Core.Domain.Exceptions;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace SGS.MultiTenancy.UI.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(
            HttpContext context,
            IGenericRepository<AuditLog> auditLogRepository,
            ICurrentUser currentUser,
            ITenantProvider tenantProvider)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                Guid logId = Guid.NewGuid();
                int statusCode = GetStatusCode(exception);

                _logger.LogError(
                    exception,
                    "Unhandled exception occurred. LogId: {LogId}",
                    logId);

                AuditLog auditLog = new AuditLog
                {
                    LogId = logId,
                    LogLevel = LogLevel.Error,
                    TimeStamp = DateTime.UtcNow,
                    Message = exception.Message,
                    StackTrace = exception.StackTrace,
                    HttpStatusCode = statusCode,
                    UserId = currentUser?.UserId,
                    TenantId = tenantProvider?.TenantId
                };

                await auditLogRepository.AddAsync(auditLog);
                await auditLogRepository.CompleteAsync();

                await HandleResponseAsync(context, logId);
            }
        }
        private static int GetStatusCode(Exception exception) => exception switch
        {
        // Handle your custom app exceptions
        AppException appEx => appEx.StatusCode,
        // Handle standard system exceptions
        UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
        KeyNotFoundException => (int)HttpStatusCode.NotFound,
        ArgumentException => (int)HttpStatusCode.BadRequest,
            DivideByZeroException => StatusCodes.Status400BadRequest,
        NotImplementedException => (int)HttpStatusCode.NotImplemented,_=> (int)HttpStatusCode.InternalServerError
     };

        private static async Task HandleResponseAsync(
            HttpContext context,
            Guid logId)
        {
            context.Response.Clear();
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.Redirect($"/ErrorP?logId={logId}");
        }
    }
}
