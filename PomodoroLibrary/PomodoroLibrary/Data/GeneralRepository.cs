using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PomodoroLibrary.Data.Database;
using PomodoroLibrary.Data.Interfaces;
using PomodoroLibrary.Models.Utility;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace PomodoroLibrary.Data;

public class GeneralRepository<T> : IRepository<T> where T : class
{
    private readonly ApplicationDbContext _db;
    internal DbSet<T> dbSet;

    public GeneralRepository(ApplicationDbContext db)
    {
        _db = db;
        dbSet = _db.Set<T>();
    }

    public async Task AddAsync(T model)
    {
        await dbSet.AddAsync(model);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        IQueryable<T> query = dbSet;
        return await query.ToListAsync();
    }

    public async Task<T?> GetAsync(Expression<Func<T, bool>> filter)
    {
        IQueryable<T> query = dbSet;
        query = query.Where(filter);
        return await query.FirstOrDefaultAsync();
    }

    public void Remove(T model)
    {
        dbSet.Remove(model);
    }

    public void RemoveRange(IEnumerable<T> models)
    {
        dbSet.RemoveRange(models);
    }
}
