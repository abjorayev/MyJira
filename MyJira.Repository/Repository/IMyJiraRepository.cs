using MyJira.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyJira.Repository.Repository
{
    public interface IMyJiraRepository<T>
    {
        Task<T> GetById(int id);
        Task<List<T>> GetAll();
        Task Add(T entity);
        Task Update(T entity);
        Task<bool> Delete(int id);
        Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate);
        Task<T> GetFirstOrDefault(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> Include(params Expression<Func<T, object>>[] includes);
    }
}
