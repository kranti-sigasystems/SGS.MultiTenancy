namespace SGS.MultiTenancy.Core.Application.Pagination
{
    public class PagedResult<T>
    {

        /// <summary>
        /// Gets or set items.
        /// </summary>
        public IReadOnlyList<T> Items { get; set; } = new List<T>();

        /// <summary>
        /// Gets or set total count.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Gets or set page numer.
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Gets or set page size.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or set total pages.
        /// </summary>
        public int TotalPages =>
            (int)Math.Ceiling(TotalCount / (double)PageSize);


        /// <summary>
        /// Gets or set has previous.
        /// </summary>
        public bool HasPrevious => PageNumber > 1;

        /// <summary>
        /// Gets or set has next.
        /// </summary>
        public bool HasNext => PageNumber < TotalPages;


        /// <summary>
        /// Gets or paged result.
        /// </summary>
        public PagedResult() { }

        /// <summary>
        /// Gets or set paged result.
        /// </summary>
        /// <param name="items"></param>
        /// <param name="totalCount"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        public PagedResult(
            IReadOnlyList<T> items,
            int totalCount,
            int pageNumber,
            int pageSize)
        {
            Items = items;
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
