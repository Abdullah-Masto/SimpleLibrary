using Library_Domain.Interfaces;
using Library_Domain.Modles.ViewModels;
using Library_Domain.Modles;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Library_Data.Repos
{
    public class Repository<T> : IRepository<T> where T : Base
    {
        protected DbContext DbContext { get; set; }

        public Repository(LibraryContext dbContext)
        {
            DbContext = dbContext;
        }

        public async void AddAsync(T entity)
        {
            await DbContext.Set<T>().AddAsync(entity);
            DbContext.SaveChanges();
        }

        public void Delete(T entity)
        {
            entity.IsDeleted = true;
            DbContext.SaveChanges();
        }

        public async Task<(List<T>, PaginationMetaData)> GetAllAsync(int pageNumber, int pageSize, Expression<Func<T, Boolean>> condition = null)
        {
            var totalItemCount = await DbContext.Set<T>().CountAsync(e => !e.IsDeleted);
            var paginationData = new PaginationMetaData(totalItemCount, pageSize, pageNumber);

            condition ??= (_ => true);

            var response = await DbContext.Set<T>()
                                .Where(condition)
                                .Where(e => !e.IsDeleted)
                                .Skip(pageSize * (pageNumber - 1))
                                .Take(pageSize)
                                .ToListAsync();

            return (response, paginationData);
        }

        public async Task<T> GetByIdAsync(int id, Expression<Func<T, object>> includes = null)
        {

            IQueryable<T> query = DbContext.Set<T>();

            if (includes != null)
            {
                query = query.Include(includes);
            }

            return await query.FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
        }

        public void Update()
        {
            DbContext.SaveChanges();
        }

        public async Task<int> GetLastIDAsync()
        {
            var last = await DbContext.Set<T>().LastOrDefaultAsync();
            return last.Id;
        }
    }
}
