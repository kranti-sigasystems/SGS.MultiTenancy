using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SGS.MultiTenancy.Core.Domain.Common
{
    /// <summary>
    /// ViewModel for table sorting and paging functionality.
    /// </summary>
    public class TableResultsRequest : IResultsSortRequest, IResultsPageRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TableResultsRequest"/> class.
        /// </summary>

        public TableResultsRequest()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableResults Request"/> class.
        /// </summary>
        /// 
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortDirection">The sort direction.</param>
        /// <param name="sortFieldName">The sort field name.</param>
        /// <param name="totalResults">The total number of rows.</param>
        public TableResultsRequest(uint? pageNumber, uint? pageSize, ListSortDirection? sortDirection, string sortFieldName, int? totalResults = null)
        {
            this.TotalResults = totalResults.HasValue ? totalResults.Value : 0;
            this.PageSize = pageSize;
            this.SortDirection = sortDirection;
            this.SortFieldName = sortFieldName;

            this.TotalPages = Math.Max(
                1,
                Convert.ToInt32(
                    Math.Ceiling((decimal)this.TotalResults / (int)this.PageSize)
                )
            );

            uint resolvedPageNumber;

            if (pageNumber.HasValue)
            {
                
                resolvedPageNumber = pageNumber.Value;

                if (resolvedPageNumber > this.TotalPages)
                {
                    resolvedPageNumber = (uint)this.TotalPages;
                }

                resolvedPageNumber = Math.Max(1, resolvedPageNumber);
            }
            else
            {
                resolvedPageNumber = 1;
            }

            this.PageNumber = resolvedPageNumber;
        }

        /// <inheritdoc/>
        public uint? PageNumber { get; set; }

        /// <inheritdoc/>
        public uint? PageSize { get; set; }

        /// <inheritdoc/>
        public ListSortDirection? SortDirection { get; set; }

        /// <inheritdoc/>
        [StringLength(100)]
        public string SortFieldName { get; set; }

        /// <summary>
        /// Gets the value for total number of pages.
        /// </summary>
        public int TotalPages { get; private set; }

        /// <summary>
        /// Gets the value for total number of rows.
        /// </summary>
        public int TotalResults { get; private set; }

        /// <summary>
        /// Set default sorting and paging values if no values exist.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortDirection">The sort direction.</param>
        /// <param name="sortFieldName">The sort field name.</param>

        public void SetDefaultSortingAndPaging(uint? pageNumber, uint? pageSize,ListSortDirection? sortDirection, string sortFieldName)
        {
            this.PageNumber ??= pageNumber;
            this.PageSize ??= pageSize;
            this.SortDirection ??= sortDirection;

            if (string.IsNullOrEmpty(this.SortFieldName))
            {
                this.SortFieldName = sortFieldName;
            }
        }

    }
}
