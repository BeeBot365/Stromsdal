using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using StromsdalKok.Infrastructure.Data;
using StromsdalKok.Infrastructure.Data.Persistence;

namespace StromsdalKok.Infrastructure.Repositories;

public abstract class GenericRepository
{
    protected readonly AppDbContext Context;

    protected GenericRepository(AppDbContext context)
    {
        Context = context;
    }

    protected async Task<TEntity?> GetByPkAsync<TEntity>(int id)
        where TEntity : BaseEntity =>
        await Context.Set<TEntity>().FindAsync(id);

    protected async Task<TEntity?> GetAsync<TEntity>(
        Expression<Func<TEntity, bool>> predicate)
        where TEntity : BaseEntity =>
        await Context.Set<TEntity>().FirstOrDefaultAsync(predicate);

    protected async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>()
        where TEntity : BaseEntity =>
        await Context.Set<TEntity>().ToListAsync();

    protected async Task<IEnumerable<TEntity>> GetManyAsync<TEntity>(
        Expression<Func<TEntity, bool>> predicate)
        where TEntity : BaseEntity =>
        await Context.Set<TEntity>().Where(predicate).ToListAsync();

    protected async Task AddEntityAsync<TEntity>(TEntity entity)
        where TEntity : BaseEntity
    {
        await Context.Set<TEntity>().AddAsync(entity);
        await Context.SaveChangesAsync();
    }

    protected async Task UpdateEntityAsync<TEntity>(int id, Action<TEntity> update)
        where TEntity : BaseEntity
    {
        var entity = await Context.Set<TEntity>().FindAsync(id);
        if (entity is null) return;
        update(entity);
        await Context.SaveChangesAsync();
    }

    protected async Task UpdateEntityAsync<TEntity>(TEntity data) where TEntity : BaseEntity
    {
        Context.Set<TEntity>().Update(data);
        await Context.SaveChangesAsync();
    }

    protected async Task DeleteEntityAsync<TEntity>(int id)
        where TEntity : BaseEntity
    {
        var entity = await Context.Set<TEntity>().FindAsync(id);
        if (entity is null) return;
        Context.Set<TEntity>().Remove(entity);
        await Context.SaveChangesAsync();
    }

    protected async Task SaveChangesAsync() =>
        await Context.SaveChangesAsync();
}