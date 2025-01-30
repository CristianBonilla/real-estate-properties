using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RealEstateProperties.Contracts.Repository;

namespace RealEstateProperties.Infrastructure.Repositories
{
  public abstract class Repository<TContext, TEntity>(IRepositoryContext<TContext> context) : IRepository<TContext, TEntity>
    where TContext : DbContext
    where TEntity : class
  {
    readonly DbSet<TEntity> _entitySet = context.Set<TEntity>();
    readonly Func<TEntity, EntityEntry<TEntity>> _entityEntry = entity => context.Entry(entity);

    public TEntity Create(TEntity entity) => _entitySet.Add(entity).Entity;

    public IEnumerable<TEntity> CreateRange(IEnumerable<TEntity> entities)
    {
      return [.. Range()];

      IEnumerable<TEntity> Range()
      {
        foreach (TEntity entity in entities)
          yield return Create(entity);
      }
    }

    public TEntity Update(TEntity entity)
    {
      var entry = _entityEntry(entity);
      if (entry.State == EntityState.Detached)
        _entitySet.Attach(entity);
      var updated = _entitySet.Update(entity);

      return updated.Entity;
    }

    public IEnumerable<TEntity> UpdateRange(IEnumerable<TEntity> entities)
    {
      return [.. Range()];

      IEnumerable<TEntity> Range()
      {
        foreach (TEntity entity in entities)
          yield return Update(entity);
      }
    }

    public TEntity Delete(TEntity entity)
    {
      var entry = _entityEntry(entity);
      if (entry.State == EntityState.Detached)
        _entitySet.Attach(entity);
      var deleted = _entitySet.Remove(entity);

      return deleted.Entity;
    }

    public IEnumerable<TEntity> DeleteRange(IEnumerable<TEntity> entities)
    {
      return [.. Range()];

      IEnumerable<TEntity> Range()
      {
        foreach (TEntity entity in entities)
          yield return Delete(entity);
      }
    }

    public TEntity? Find(object[] keyValues, params Expression<Func<TEntity, object>>[] navigations)
    {
      TEntity? found = _entitySet.Find(keyValues);

      return found is not null ? WithNavigations(navigations).SingleOrDefault(entity => entity == found) : null;
    }

    public TEntity? Find(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] navigations) => WithNavigations(navigations).FirstOrDefault(predicate);

    public bool Exists(Expression<Func<TEntity, bool>> predicate) => _entitySet.Any(predicate);

    public IEnumerable<TEntity> GetAll(
      Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
      params Expression<Func<TEntity, object>>[] navigations) => [.. orderBy is not null ? orderBy(WithNavigations(navigations)).AsQueryable() : WithNavigations(navigations)];

    public IEnumerable<TEntity> GetByFilter(
      Expression<Func<TEntity, bool>> filter,
      Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
      params Expression<Func<TEntity, object>>[] navigations) => [.. (orderBy is not null ? orderBy(WithNavigations(navigations)) : WithNavigations(navigations)).Where(filter)];

    private IQueryable<TEntity> WithNavigations(params Expression<Func<TEntity, object>>[] navigations)
    {
      if (navigations.Length == 0)
        return _entitySet;

      var querySet = _entitySet.AsQueryable();
      foreach (var expression in navigations)
        querySet = querySet.Include(expression);

      return querySet;
    }
  }
}
