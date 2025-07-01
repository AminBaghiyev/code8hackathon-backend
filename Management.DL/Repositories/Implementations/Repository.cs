using Management.Core.Entities.Base;
using Management.DL.Contexts;
using Management.DL.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Management.DL.Repositories.Implementations;

public class Repository<T> : IRepository<T> where T : BaseEntity, new()
{
    protected readonly AppDbContext _context;

    public Repository(AppDbContext context)
    {
        _context = context;
    }

    public DbSet<T> Table => _context.Set<T>();

    public async Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, int page = 0, int count = 0, bool orderAsc = true, string orderByProperty = "Id", Func<IQueryable<T>, IIncludableQueryable<T, object>>? includes = null)
    {
        IQueryable<T> query = Table.AsNoTracking();

        if (includes is not null) query = includes(query);

        if (predicate is not null) query = query.Where(predicate);

        ParameterExpression parameter = Expression.Parameter(typeof(T), "e");
        MemberExpression property = Expression.Property(parameter, orderByProperty);
        Expression<Func<T, object>> lambda = Expression.Lambda<Func<T, object>>(Expression.Convert(property, typeof(object)), parameter);

        query = orderAsc ? query.OrderBy(lambda) : query.OrderByDescending(lambda);

        if (count > 0) query = query.Skip(page * count).Take(count);

        return await query.ToListAsync();
    }

    public async Task<T?> GetOneAsync(Expression<Func<T, bool>> predicate, bool isTracking = false, Func<IQueryable<T>, IIncludableQueryable<T, object>>? includes = null)
    {
        IQueryable<T> query = Table;

        if (!isTracking) query = query.AsNoTracking();

        if (includes is not null) query = includes(query);

        return await query.SingleOrDefaultAsync(predicate);
    }

    public async Task CreateAsync(T entity)
    {
        await Table.AddAsync(entity);
    }

    public void Update(T entity)
    {
        Table.Update(entity);
    }

    public void Delete(T entity)
    {
        Table.Remove(entity);
    }

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
}
