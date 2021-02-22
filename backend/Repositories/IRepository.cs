using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Repositories
{
	public interface IRepository<TEntity, in TKey> : IDisposable
	where TEntity : class
	where TKey : IEquatable<TKey>
	{
		public Task<int> GetCount();
		public Task<TEntity> FindById(TKey id);
		public Task<IList<TEntity>> GetAll();

		public Task<IList<TEntity>> GetPagified(int page, int resultsPerPage, Expression<Func<TEntity, bool>> predicate);

		public Task<IList<TEntity>> GetPagified(int page, int resultsPerPage);

		public Task Create(TEntity entity);

		public Task Update(TEntity entity);

		public Task Delete(TKey id);

		public Task<TEntity> FindByProperty(Expression<Func<TEntity, bool>> predicate);

		public Task<IList<TEntity>> FindManyByProperty(Expression<Func<TEntity, bool>> predicate);
	}
}