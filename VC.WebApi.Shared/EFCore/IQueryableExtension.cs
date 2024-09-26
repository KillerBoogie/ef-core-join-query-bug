using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Text;

namespace VC.WebApi.SharedKernel.Persistence
{
    //https://stackoverflow.com/questions/298725/multiple-order-by-in-linq/44438959#44438959  sjkm
    public static class IQueryableExtension
    {
        public static bool IsOrdered<T>(this IQueryable<T> queryable)
        {
            return queryable.Expression.Type == typeof(IOrderedQueryable<T>);
        }

        public static IQueryable<T> SmartOrderBy<T, TKey>(this IQueryable<T> queryable, Expression<Func<T, TKey>> keySelector)
        {
            if (queryable.IsOrdered())
            {
                var orderedQuery = queryable as IOrderedQueryable<T>;
                return orderedQuery!.ThenBy(keySelector);
            }
            else
            {
                return queryable.OrderBy(keySelector);
            }
        }

        public static IQueryable<T> SmartOrderByDescending<T, TKey>(this IQueryable<T> queryable, Expression<Func<T, TKey>> keySelector)
        {
            if (queryable.IsOrdered())
            {
                var orderedQuery = queryable as IOrderedQueryable<T>;
                return orderedQuery!.ThenByDescending(keySelector);
            }
            else
            {
                return queryable.OrderByDescending(keySelector);
            }
        }

        /// <summary>
        /// Add sorting to the given query for all Values in sortingMap
        /// <para />The last char of each sort value may be "+" oder "-" to indicate ascending/desending search
        /// <para />Every sort value, except final "+" or "-" must be part of columnMap otherwise throws ElwisQueryException 
        /// </summary>
        public static IQueryable<T> ApplySort<T>(this IQueryable<T> query, IEnumerable<string> sortingMap, Dictionary<string, Expression<Func<T, object>>> columnsMap)
        {
            foreach (string sortingColumn in sortingMap)
            {
                string lastChar = sortingColumn.Substring(sortingColumn.Length - 1, 1);
                bool sortDesc = (lastChar == "-");

                string column;
                if (lastChar == "-" || lastChar == "+")
                {
                    column = sortingColumn.Substring(0, sortingColumn.Length - 1).ToLower();
                }
                else
                {
                    column = sortingColumn.ToLower();
                }

                if (columnsMap.ContainsKey(column))
                {
                    if (sortDesc)
                    {
                        query = query.SmartOrderByDescending(columnsMap[column]);
                    }
                    else
                    {
                        query = query.SmartOrderBy(columnsMap[column]);
                    }
                }
                else
                {
                    StringBuilder sb = new StringBuilder();

                    foreach (string sortingMapEntry in sortingMap)
                    {
                        sb.Append(" - sortingMap: \"" + sortingMapEntry + "\"");
                    }
                    foreach (var columnsMapEntry in columnsMap)
                    {
                        sb.Append(" - columnsMap: \"" + columnsMapEntry.Key + "\"");
                    }
                    throw new ApplicationException(sb.ToString());
                }
            }
            return query;
        }

        public static IQueryable<T> ApplySearch<T>(this IQueryable<T> query, Expression<Func<T, bool>>? searchCriteria)
        {
            if (searchCriteria is null)
            {
                return query;
            }
            return query.Where(searchCriteria);
        }

        public static IQueryable<T> ForUpdate<T>(this IQueryable<T> query)
        {
            return query.TagWith("ForUpdate");
        }

        public static IQueryable<T> ForUpdateHold<T>(this IQueryable<T> query)
        {
            return query.TagWith("ForUpdateHold");
        }

        public static IQueryable<T> ForTableLock<T>(this IQueryable<T> query)
        {
            return query.TagWith("ForTableLock");
        }

        //public static Result<PagedResultDTO<T>> Paged<T>(this IQueryable<T> query, bool? paged,
        //                                 PageNumber? pageNumber, PageSize? pageSize) where T : class
        //{
        //    if (paged is null)
        //    {
        //        throw new ArgumentNullException("Paging paramater 'paged' is not set!");
        //    }
        //    if (paged is true && pageNumber is null)
        //    {
        //        throw new ArgumentNullException("Paging is activated, but paramater 'pageNumber' is not set!");
        //    }
        //    if (paged is true && pageNumber is null)
        //    {
        //        throw new ArgumentNullException("Paging is activated, but paramater 'pageSize' is not set!");
        //    }

        //    IList<T> results = new List<T>();
        //    int? pageCount = null;
        //    int? firstItemNumberOnPage = null;
        //    int? lastItemNumberOnPage = null;

        //    int totalCount = query.Count();

        //    if (paged == true)
        //    {
        //        pageCount = pageSize!.Value == 0 || totalCount == 0 ? 1 : (int)Math.Ceiling(totalCount / (double)pageSize!.Value);

        //        if (pageNumber!.Value > pageCount)
        //        {
        //            return Result<PagedResultDTO<T>>.Failure(Error.NotFound().Page_Not_Found().Param(nameof(pageNumber), pageNumber!.Value).Param(nameof(pageCount), pageCount!.Value));
        //        }

        //        firstItemNumberOnPage = (pageNumber!.Value - 1) * pageSize!.Value + 1;
        //        lastItemNumberOnPage = pageSize!.Value == 0 ? totalCount : Math.Min(pageNumber!.Value * pageSize!.Value, totalCount);

        //        int skip = (pageNumber!.Value - 1) * pageSize!.Value;
        //        query = query.Skip(skip).Take(pageSize.Value);

        //        results = query.ToList();
        //    }
        //    else if (paged == false)
        //    {
        //        results = query.ToList();
        //    }

        //    Result<PagedResultDTO<T>> result = Result<PagedResultDTO<T>>.Success(new PagedResultDTO<T>(results, totalCount, pageNumber, pageSize, pageCount, firstItemNumberOnPage, lastItemNumberOnPage));
        //    return result;
        //}
    }
}
