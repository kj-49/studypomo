using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroLibrary.Data.Interfaces;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetAsync(Expression<Func<T, bool>> filter);
    Task AddAsync(T model);
    void Remove(T model);
    void RemoveRange(IEnumerable<T> models);
}
