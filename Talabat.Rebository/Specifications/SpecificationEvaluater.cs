using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Infrastructure.Specifications
{
    public class SpecificationEvaluater<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecifications<T> specifications)
        {
            var query = inputQuery;

            if (specifications.Criteria is not null)
                query = query.Where(specifications.Criteria);
            
            if (specifications.OrderBy is not null)
                query = query.OrderBy(specifications.OrderBy);
            
            if (specifications.OrderByDesc is not null)
                query = query.OrderByDescending(specifications.OrderByDesc);

            if (specifications.IsPaginated)
                query = query.Skip(specifications.Skip).Take(specifications.Take);

            query = specifications.Includes.Aggregate(query, (current, include) => current.Include(include));

            return query;
        }
    }

}
