using System.Linq.Expressions;

namespace Talabat.Infrastructure.Specifications
{
    public class BaseSpecification<T> : ISpecifications<T>
    {
        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        public Expression<Func<T, bool>> Criteria { get; }

        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();

        public Expression<Func<T, object>> OrderBy  { get; private set; }

        public Expression<Func<T, object>> OrderByDesc { get; private set; }

        public int Take { get; private set; }

        public int Skip { get; private set; }

        public bool IsPaginated { get; private set; }

        protected void AddInclude(Expression<Func<T, object>> includeExpression) 
            => Includes.Add(includeExpression); 
        protected void AddOrderBy(Expression<Func<T, object>> orderby)
            => OrderBy = orderby;
        protected void AddOrderByDesc(Expression<Func<T, object>> orderbydesc)
            => OrderByDesc = orderbydesc;
        protected void ApplyPaginaton(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPaginated = true;
        }

    }
}
