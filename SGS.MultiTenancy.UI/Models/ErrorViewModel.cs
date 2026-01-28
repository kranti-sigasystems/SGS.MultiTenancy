namespace SGS.MultiTenancy.UI.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public Guid LogId { get; set; }
        public string ErrorMessage { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
