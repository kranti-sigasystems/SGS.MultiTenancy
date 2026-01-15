
namespace SGS.MultiTenancy.Core.Application.DTOs
{
    public class GenericResponseDto<T>
    {
        /// <summary>
        /// Response message
        /// </summary>
        public string? Message { get; set; } = string.Empty;

        /// <summary>
        /// Single object (for GetById or Create/Update response)
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// Optional list of objects (for Get/List operations)
        /// </summary>
        public List<T>? Items { get; set; }

        /// <summary>
        /// Pagination info
        /// </summary>
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages => PageSize > 0 ? (int)Math.Ceiling((double)TotalCount / PageSize) : 0;
    }

}
