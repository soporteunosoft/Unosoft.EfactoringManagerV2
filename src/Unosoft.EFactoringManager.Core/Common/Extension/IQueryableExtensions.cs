using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Unosoft.EFactoringManager.Common.Extension
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> ApplyOrdering<T>(
            this IQueryable<T> query,
            string field,
            int sortOrder,
            Dictionary<string, Expression<Func<T, object>>> map,
            Expression<Func<T, object>> defaultOrder)
        {
            var ascending = sortOrder >= 0;

            if (!string.IsNullOrWhiteSpace(field) && map.TryGetValue(field, out var expr))
                return ascending
                    ? query.OrderBy(expr)
                    : query.OrderByDescending(expr);

            return ascending
                     ? query.OrderBy(defaultOrder)
                     : query.OrderByDescending(defaultOrder);
        }
    }

}
