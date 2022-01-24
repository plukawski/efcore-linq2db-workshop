using System;
using System.Linq;
using System.Linq.Expressions;

namespace NorthwindDataAccess.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> ConditionalWhere<T>(
            this IQueryable<T> source,
            Func<bool> condition,
            Expression<Func<T, bool>> predicate)
        {
            if (condition())
            {
                return source.Where(predicate);
            }

            return source;
        }
    }
}
