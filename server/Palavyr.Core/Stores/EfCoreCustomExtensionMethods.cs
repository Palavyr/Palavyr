using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Palavyr.Core.Stores
{
    public static class EfCoreCustomExtensionMethods
    {
        public static IQueryable<TEntity> CustomWhere<TEntity>(
            this IQueryable<TEntity> query,
            string id,
            Expression<Func<TEntity, string>> propertySelectorExpression)
        {
            if (string.IsNullOrEmpty(id))
            {
                return query;
            }

            return InnerWhere(query, propertySelectorExpression, Expression.Constant($"%{id}%"));
        }

        private static IQueryable<TEntity> InnerWhere<TEntity>(
            this IQueryable<TEntity> query,
            Expression<Func<TEntity, string>> propertySelectorExpression,
            ConstantExpression searchValueConstant
        )
        {
            if (!(propertySelectorExpression.Body is MemberExpression memberExpression))
            {
                return query;
            }

            // get method info for EF.Functions.Like
            var likeMethod = typeof(DbFunctionsExtensions).GetMethod(
                nameof(DbFunctionsExtensions.Like), new[]
                {
                    typeof(DbFunctions),
                    typeof(string),
                    typeof(string)
                });
            var dbFunctionsConstant = Expression.Constant(EF.Functions);
            var propertyInfo = typeof(TEntity).GetProperty(memberExpression.Member.Name);
            var parameterExpression = Expression.Parameter(typeof(TEntity));
            var propertyExpression = Expression.Property(parameterExpression, propertyInfo);


            var callLikeExpression = Expression.Call(likeMethod, dbFunctionsConstant, propertyExpression, searchValueConstant);
            var lambda = Expression.Lambda<Func<TEntity, bool>>(callLikeExpression, parameterExpression);
            return query.Where(lambda);
        }

        // public static IQueryable<TEntity> CustomWhere<TEntity>(
        //     this IQueryable<TEntity> query,
        //     IEnumerable<string> ids,
        //     Expression<Func<TEntity, string>> propertySelectorExpression)
        // {
        //     ids = ids.ToArray();
        //     if (ids.Count() == 0)
        //     {
        //         return query;
        //     }
        //
        //     return InnerWhere(query, propertySelectorExpression, Expression.Constant($"%{string.Join(",", ids)}%"));
        // }
    }
}