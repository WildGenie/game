using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
	public class Repository<TEntity, TKey> : IRepository<TEntity, TKey>
		where TEntity : class
		where TKey : IEquatable<TKey>
	{
		protected readonly ApplicationDbContext Context;

		public Repository(ApplicationDbContext context)
		{
			Context = context;
		}

		[ExcludeFromCodeCoverage]
		public void Dispose() { }

		public async Task<int> GetCount()
		{
			return await Context.Set<TEntity>()
								.CountAsync();
		}

		public async Task<TEntity> FindById(TKey id)
		{
			return await Context.Set<TEntity>()
								.FindAsync(id);
		}

		public async Task<IList<TEntity>> GetAll()
		{
			return await Context.Set<TEntity>()
								.ToListAsync();
		}

		public async Task<IList<TEntity>> GetPagified(int page, int resultsPerPage, Expression<Func<TEntity, bool>> predicate)
		{
			var result = predicate == null
				? Context.Set<TEntity>()
				: Context.Set<TEntity>()
						 .Where(predicate);

			return await result.Skip(--page * resultsPerPage)
							   .Take(resultsPerPage)
							   .ToListAsync();
		}

		public Task<IList<TEntity>> GetPagified(int page, int resultsPerPage)
		{
			return GetPagified(page, resultsPerPage, null);
		}

		public async Task Create(TEntity entity)
		{
			await Context.Set<TEntity>()
						 .AddAsync(entity);
			await Context.SaveChangesAsync();
		}

		public async Task Update(TEntity entity)
		{
			Context.Set<TEntity>()
				   .Update(entity);
			await Context.SaveChangesAsync();
		}

		public async Task Delete(TKey id)
		{
			var entity = await Context.Set<TEntity>()
									  .FindAsync(id);
			if (entity == null)
			{
				throw GenerateNotFoundException();
			}

			Context.Set<TEntity>()
				   .Remove(entity);
			await Context.SaveChangesAsync();
		}

		public async Task<TEntity> FindByProperty(Expression<Func<TEntity, bool>> predicate)
		{
			return await Context.Set<TEntity>()
								.FirstOrDefaultAsync(predicate);
		}

		public async Task<IList<TEntity>> FindManyByProperty(Expression<Func<TEntity, bool>> predicate)
		{
			return await Context.Set<TEntity>()
								.Where(predicate)
								.ToListAsync();
		}

		private static KeyNotFoundException GenerateNotFoundException(string message = "The requested entity was not found.")
		{
			return new KeyNotFoundException(message);
		}
	}
}