namespace SGS.MultiTenancy.UI.Models
{
    public class ErrorViewModel
    {
        /// <summary>
        /// Gets or set request Id.
        /// </summary>
        public string? RequestId { get; set; }

        /// <summary>
        /// Gets or set a value indicating whether to show request Id.
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
