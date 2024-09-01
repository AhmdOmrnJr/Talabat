using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Infrastructure.Specifications;

namespace Talabat.Infrastructure.Interfaces
{
    public interface IGenericRepository<T> where T: BaseEntity
    {
        Task<T> GetByIdAsync(int? id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetEntityWithSpecifictionAsync(ISpecifications<T> specs);
        Task<IReadOnlyList<T>> GetAllWithSpecifictionAsync(ISpecifications<T> specs);
        Task<int> CountAsync(ISpecifications<T> specifications);
        Task Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
