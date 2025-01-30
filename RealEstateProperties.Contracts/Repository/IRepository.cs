using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace RealEstateProperties.Contracts.Repository
{
  public interface IRepository<in TContext, TEntity>
    where TContext : DbContext
    where TEntity : class
  {
    TEntity Create(TEntity entity);
    IEnumerable<TEntity> CreateRange(IEnumerable<TEntity> entities);
    TEntity Update(TEntity entity);
    IEnumerable<TEntity> UpdateRange(IEnumerable<TEntity> entities);
    TEntity Delete(TEntity entity);
    IEnumerable<TEntity> DeleteRange(IEnumerable<TEntity> entities);
    TEntity? Find(object[] keyValues, params Expression<Func<TEntity, object>>[] navigations);
    TEntity? Find(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] navigations);
    bool Exists(Expression<Func<TEntity, bool>> predicate);
    IEnumerable<TEntity> GetAll(
      Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
      params Expression<Func<TEntity, object>>[] navigations);
    IEnumerable<TEntity> GetByFilter(
      Expression<Func<TEntity, bool>> filter,
      Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
      params Expression<Func<TEntity, object>>[] navigations);
  }
}
