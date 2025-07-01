using Management.Core.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Management.DL.Repositories.Abstractions;

public interface IRepository<T> where T : BaseEntity, new()
{
    DbSet<T> Table { get; }
    Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, int page = 0, int count = 0, bool orderAsc = true, string orderByProperty = "Id", Func<IQueryable<T>, IIncludableQueryable<T, object>>? includes = null);
    Task<T?> GetOneAsync(Expression<Func<T, bool>> predicate, bool isTracking = false, Func<IQueryable<T>, IIncludableQueryable<T, object>>? includes = null);
    Task CreateAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task<int> SaveChangesAsync();
}
