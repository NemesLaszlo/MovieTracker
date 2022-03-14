using Microsoft.EntityFrameworkCore;

namespace MovieTracker_API.Extensions
{
    public static class HttpContextExtensions
    {
        public async static Task InsertParametersPaginationInHeader<T>(this HttpContext httpContext, IQueryable<T> queryable)
        {

            if (httpContext == null) { throw new ArgumentNullException(nameof(httpContext)); }

            double count = await queryable.CountAsync();
            httpContext.Response.Headers.Add("totalAmountOfRecords", count.ToString());
        }
    }
}
