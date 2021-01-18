using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace Utils
{
    public static class LinqExpression
    {
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool conditionIsValid, Expression<Func<T, bool>> query)
        {
            if (conditionIsValid)
            {
                return source.Where(query);
            }

            return source;
        }
    }
}