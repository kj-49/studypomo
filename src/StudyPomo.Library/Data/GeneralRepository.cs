using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StudyPomo.Library.Data.Database;
using StudyPomo.Library.Data.Interfaces;
using StudyPomo.Library.Models.Utility;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace StudyPomo.Library.Data;

public class GeneralRepository<T> : IRepository<T> where T : class
{
    private readonly ApplicationDbContext _db;
    internal DbSet<T> dbSet;

    public GeneralRepository(ApplicationDbContext db)
    {
        _db = db;
        dbSet = _db.Set<T>();
    }

    public async Task<T> AddAsync(T model)
    {
        return (await dbSet.AddAsync(model)).Entity;
    }


    public async Task AddRangeAsync(IEnumerable<T> models)
    {
        await dbSet.AddRangeAsync(models);
    }
    public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> query = dbSet;

        foreach (var includeProperty in includeProperties)
        {
            query = query.Include(includeProperty);
        }

        return await query.AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> query = dbSet;

        query = query.Where(filter);

        foreach (var includeProperty in includeProperties)
        {
            query = query.Include(includeProperty);
        }

        return await query.AsNoTracking().ToListAsync();
    }

    public async Task<T?> GetAsync(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> query = dbSet;

        query = query.Where(filter);

        foreach (var includeProperty in includeProperties)
        {
            query = query.Include(includeProperty);
        }

        return await query.AsNoTracking().FirstOrDefaultAsync();
    }

    public void Remove(T model)
    {
        dbSet.Remove(model);
    }

    public void RemoveRange(IEnumerable<T> models)
    {
        if (models.Any())
        {
            dbSet.RemoveRange(models);
        }
    }
}
