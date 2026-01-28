using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using SGS.MultiTenancy.Core.Application.Interfaces;
using SGS.MultiTenancy.Core.Domain.Entities.Auth;
using SGS.MultiTenancy.Core.Domain.Exceptions;

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

                _logger.LogError(exception,
                    "Unhandled exception occurred. LogId: {LogId}", logId);

                var logLevel = ResolveLogLevel(exception);

                // STEP A: sanitize message BEFORE saving to DB
                string safeMessage = IsDatabaseException(exception)
                    ? "A database connectivity error occurred."
                    : exception.Message;

                // STEP B: create audit log using safe message
                var auditLog = new AuditLog
                {
                    LogId = logId,
                    LogLevel = logLevel,
                    TimeStamp = DateTime.UtcNow,
                    Message = safeMessage,
                    StackTrace = exception.StackTrace,
                    HttpStatusCode = statusCode,
                    UserId = currentUser?.UserId,
                    TenantId = tenantProvider?.TenantId
                };



                bool auditLogged = false;

                try
                {
                    await auditLogRepository.AddAsync(auditLog);
                    await auditLogRepository.CompleteAsync();
                    auditLogged = true;
                }
                catch (Exception dbEx)
                {
                    _logger.LogCritical(dbEx,
                        "AUDIT LOG FAILURE. Original Error LogId: {LogId}", logId);

                }

                await HandleResponseAsync(context, logId);
            }
        }

        private static bool IsDatabaseException(Exception ex)
        {
            return ex is DbUpdateException
                || ex is TimeoutException
                || ex is MySqlException;
        }


        private static LogLevel ResolveLogLevel(Exception exception)
        {
            return exception switch
            {
                AppException appEx when appEx.StatusCode < 500 => LogLevel.Warning,

                DbUpdateException => LogLevel.Critical,
                TimeoutException => LogLevel.Critical,
                MySqlException => LogLevel.Critical,

                UnauthorizedAccessException => LogLevel.Warning,
                ArgumentException => LogLevel.Warning,

                _ => LogLevel.Critical
            };
        }


        private static int GetStatusCode(Exception exception) => exception switch
        {
            AppException appEx => appEx.StatusCode,

            DbUpdateException => StatusCodes.Status503ServiceUnavailable,
            TimeoutException => StatusCodes.Status504GatewayTimeout,

            MySqlException => StatusCodes.Status503ServiceUnavailable,

            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            ArgumentException => StatusCodes.Status400BadRequest,

            _ => StatusCodes.Status500InternalServerError
        };

        private static async Task HandleResponseAsync(HttpContext context, Guid logId)
        {
            var tempDataFactory =
                context.RequestServices.GetRequiredService<ITempDataDictionaryFactory>();
            var tempData = tempDataFactory.GetTempData(context);

            var returnUrl = tempData["ReturnUrl"]?.ToString();

            if (string.IsNullOrWhiteSpace(returnUrl) ||
                !Uri.IsWellFormedUriString(returnUrl, UriKind.Relative))
            {
                returnUrl = "/";
            }

            context.Response.Clear();
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            context.Response.Redirect(
                $"/ErrorP?logId={logId}&returnUrl={Uri.EscapeDataString(returnUrl)}");

            await Task.CompletedTask;
        }

    }
}
