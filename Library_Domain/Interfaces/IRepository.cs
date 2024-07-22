using Library_Domain.Modles;
using Library_Domain.Modles.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Library_Domain.Interfaces
{
    public interface IRepository<T> where T : Base
    {
        void AddAsync(T entity);
        Task<(List<T>, PaginationMetaData)> GetAllAsync(int pageNumber, int pageSize, Expression<Func<T, Boolean>> condition = null);
        Task<T> GetByIdAsync(int id, Expression<Func<T, object>> includes = null);
        Task<int> GetLastIDAsync();
        void Update();
        void Delete(T entity);

    }
}
