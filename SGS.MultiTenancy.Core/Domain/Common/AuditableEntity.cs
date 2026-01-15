namespace SGS.MultiTenancy.Core.Domain.Common
{
    /// <summary>
    /// Represents a base entity that includes audit information.
    /// </summary>
    public abstract class AuditableEntity
    {
        /// <summary>
        /// Gets or set create by
        /// </summary>12
        public Guid CreateBy { get; set; }

        /// <summary>
        /// Gets or set update by
        /// </summary>
        public Guid? UpdateBy { get; set; }

        /// <summary>
        /// Gets or set update on
        /// </summary>
        public DateTime? UpdateOn { get; set; }

        /// <summary>
        /// Gets or set create on
        /// </summary>
        public DateTime CreateOn { get; set; }

        /// <summary>
        /// Gets or set IsDeleted
        /// </summary>
        public bool IsDeleted { get; set; } = false;
        }
}
