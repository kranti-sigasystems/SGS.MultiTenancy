using Microsoft.Extensions.Logging;
using SGS.MultiTenancy.Core.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace SGS.MultiTenancy.Core.Domain.Entities.Auth
{
    /// <summary>
    /// Represents an audit log entry used to record system events and user actions
    /// within a multi-tenant application. It captures log severity, timestamp.
    /// </summary>
    public class AuditLog 
    {
        /// <summary>
        /// Gets or sets unique identifier for the log entry.
        /// </summary>
        [Key]
        public Guid LogId { get; set; }

        /// <summary>
        /// Severity level of the log.
        /// </summary>
        [Required]
        public LogLevel LogLevel { get; set; }

        /// <summary>
        /// Gets and sets timestamp when the log entry was created (UTC).
        /// </summary>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// Identifier of the tenant associated with the log, if applicable.
        /// </summary>
        public Guid? TenantId { get; set; }

        /// <summary>
        /// Identifier of the tenant associated with the log, if applicable.
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// Detailed message describing the log event.
        /// </summary>
        [Required]
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// HTTP status code related to the log event, if any.
        /// </summary>
        public int? HttpStatusCode { get; set; }
        /// <summary>
        /// Stack trace information for error logs.
        /// </summary>
        public string? StackTrace { get; set; }
    }
}
