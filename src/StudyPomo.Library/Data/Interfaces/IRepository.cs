using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StudyPomo.Library.Data.Interfaces;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties);
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includeProperties);
    Task<T?> GetAsync(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includeProperties);
    Task<T> AddAsync(T model);
    void Remove(T model);
    void RemoveRange(IEnumerable<T> models);
}
