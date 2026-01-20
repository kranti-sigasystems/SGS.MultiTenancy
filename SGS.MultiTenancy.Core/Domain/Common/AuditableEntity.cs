namespace SGS.MultiTenancy.Core.Domain.Common
{
    /// <summary>
    /// Represents a base entity that includes audit information.
    /// </summary>
    public abstract class AuditableEntity
    {
        /// <summary>
        /// Gets or set create by
        /// </summary>
        public Guid CreateBy { get; set; }

        /// <summary>
        /// Gets or set update by
        /// </summary>
        public Guid? LastUpdateBy { get; set; }

        /// <summary>
        /// Gets or set update on
        /// </summary>
        public DateTime? LastUpdateOn { get; set; }

        /// <summary>
        /// Gets or set create on
        /// </summary>
        public DateTime CreateOn { get; set; }
    }
}
