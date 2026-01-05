namespace SGS.MultiTenancy.Core.Domain.Common
{
    public abstract class AuditableEntity
    {
        /// <summary>
        /// Gets or set create by
        /// </summary>
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
    }
}
