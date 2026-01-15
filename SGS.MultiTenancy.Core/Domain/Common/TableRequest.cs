
using System.ComponentModel;

namespace SGS.MultiTenancy.Core.Domain.Common
{
    public class TableRequest
    {
        public string? sortListDirection;
        public string? sortFieldName;
        public int? totalResults = null;
        public int PageSize;
        public int TotalPages;
        public uint PageNumber;
        public TableRequest(string? sortListDirection, string? sertFieldName, int? totalResults, int pageSize, int totalPages, uint pageNumber)
        {
            this.sortListDirection = sortListDirection;
            this.sortFieldName = sertFieldName;
            this.totalResults = totalResults;
            this.PageSize = pageSize;
            TotalPages = totalPages;
            PageNumber = pageNumber;
        }
        public void SetDefaultSortingAndPaging(uint? pageNumber, uint? pageSize, ListSortDirection? sortDirection, string sortFieldName)
        {

        }
    }
}
