using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Talabat.Core.Context;
using Talabat.Core.Entities;
using Talabat.Infrastructure.Interfaces;
using Talabat.Infrastructure.Specifications;

namespace Talabat.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreDbContext _context;

        public GenericRepository(StoreDbContext context)
        {
            _context = context;
        }
        public async Task Add(T entity)
            => await _context.AddAsync(entity);

        public void Delete(T entity)
            => _context.Remove(entity);

        public async Task<IReadOnlyList<T>> GetAllAsync()
            => await _context.Set<T>().ToListAsync();

        public async Task<T> GetByIdAsync(int? id)
            => await _context.Set<T>().FindAsync(id);

        public void Update(T entity)
            => _context.Update(entity);
        public async Task<T> GetEntityWithSpecifictionAsync(ISpecifications<T> specs)
            => await ApplySpecification(specs).FirstOrDefaultAsync();

        public async Task<IReadOnlyList<T>> GetAllWithSpecifictionAsync(ISpecifications<T> specs)
            => await ApplySpecification(specs).ToListAsync();

        private IQueryable<T> ApplySpecification(ISpecifications<T> specs)
            => SpecificationEvaluater<T>.GetQuery(_context.Set<T>().AsQueryable(), specs);

        public async Task<int> CountAsync(ISpecifications<T> specifications)
            => await ApplySpecification(specifications).CountAsync();
    }
}
