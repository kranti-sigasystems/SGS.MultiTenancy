using Microsoft.EntityFrameworkCore;
using SGS.MultiTenancy.Core.Application.Pagination;

namespace SGS.MultiTenancy.Core.Extensions
{
    public static class QueryableExtensions
    {
        /// <summary>
        /// Retrive the paged results.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="paginationParams"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>PagedResult<T> </returns>
        public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
            this IQueryable<T> query,
            PaginationParams paginationParams,
            CancellationToken cancellationToken = default)
        {
            int totalCount = await query.CountAsync(cancellationToken);

            List<T> items = await query
                .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<T>(
                items,
                totalCount,
                paginationParams.PageNumber,
                paginationParams.PageSize);
        }
    }
}
