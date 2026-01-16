namespace SGS.MultiTenancy.Core.Domain.Common
{
    /// <summary>
    /// Contract for a view model that provides paging capabilities
    /// on results of a request.
    /// </summary>
    public interface IResultsPageRequest
    {
        /// <summary>
        /// Gets or sets the page number (1-based index).
        /// </summary>
        uint? PageNumber { get; set; }

        /// <summary>
        /// Gets or sets the number of results per page.
        /// </summary>
        uint? PageSize { get; set; }
    }
}