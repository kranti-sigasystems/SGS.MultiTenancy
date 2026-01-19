namespace SGS.MultiTenancy.Core.Domain.Common
{
    public class ModalHeaderViewModel
    {
        public string Title { get; set; }
        public string? IconClass { get; set; }

        public ModalHeaderViewModel(string title, string? iconClass = null)
        {
            Title = title;
            IconClass = iconClass;
        }
    }
}
