namespace SGS.MultiTenancy.Core.Domain.Common
{
    public class ModalHeaderViewModel
    {
        /// <summary>
        /// Gets or sets title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets Icon class.
        /// </summary>
        public string? IconClass { get; set; }

        /// <summary>
        /// Gets or sets the CSS class for the header.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="iconClass"></param>
        public ModalHeaderViewModel(string title, string? iconClass = null)
        {
            Title = title;
            IconClass = iconClass;
        }
    }
}