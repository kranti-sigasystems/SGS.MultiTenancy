using System.ComponentModel;
namespace SGS.MultiTenancy.Core.Domain.Common
{
    /// <summary>
    /// Contract for a view model that provides sorting capabilities
    /// on results of a request.
    /// </summary>
    public interface IResultsSortRequest
    {
        /// <summary>
        /// Gets or sets the name of the sort field.
        /// </summary>
        string SortFieldName { get; set; }

        /// <summary>
        /// Gets or sets the sort direction.
        /// </summary>
        ListSortDirection? SortDirection { get; set; }
    }
}