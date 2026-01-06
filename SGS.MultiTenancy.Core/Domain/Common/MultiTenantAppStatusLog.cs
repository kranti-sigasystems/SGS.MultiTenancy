using SGS.MultiTenancy.Core.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace SGS.MultiTenancy.Core.Domain.Common
{
    /// <summary>
    /// Represents a log entry for application status in a multi-tenant environment.
    /// </summary>
    public class MultiTenantAppStatusLog
    {
        /// <summary>
        /// Unique identifier for the log entry.
        /// </summary>
        [Key]
        public Guid LogId { get; set; }

        /// <summary>
        /// Severity level of the log.
        /// </summary>
        [Required]
        public LogLevel LogLevel { get; set; }

        /// <summary>
        /// Timestamp when the log entry was created (UTC).
        /// </summary>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// Identifier of the tenant associated with the log, if applicable.
        /// </summary>
        public Guid? TenantId { get; set; }

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
        /// Information about the user who triggered the event, if available.
        /// </summary>
        [MaxLength(500)]
        public string? UserInfo { get; set; }
    }
}
