using SGS.MultiTenancy.Core.Application.Pagination;
using SGS.MultiTenancy.Core.Domain.Enums;

namespace SGS.MultiTenancy.UI.Models
{
    public class PagedListViewModel<T>
    {
        /// <summary>
        /// Gets or set Data.
        /// </summary>
        public PagedResult<T> Data { get; set; } = default!;

        /// <summary>
        /// Gets or set search term.
        /// </summary>
        public string? SearchTerm { get; set; }

        /// <summary>
        /// Gets or set status.
        /// </summary>
        public EntityStatus? Status { get; set; }
    }
}
