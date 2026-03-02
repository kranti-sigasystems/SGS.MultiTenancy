namespace SGS.MultiTenancy.Core.Application.Pagination
{
    public class PaginationParams
    {
        /// <summary>
        /// Gets or set max page size.
        /// </summary>
        private const int MaxPageSize = 100;

        /// <summary>
        /// Gets or set page. 
        /// </summary>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// Gets or set _pageSize.
        /// </summary>
        private int _pageSize = 5;

        /// <summary>
        /// Gets or set _PageSize.
        /// </summary>
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
    }
}
