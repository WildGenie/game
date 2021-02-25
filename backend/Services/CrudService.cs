using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using Repositories;

namespace Services
{
	public class CrudService<TEntity, TRepo> : ICrudService<TEntity>
		where TEntity : class, new()
		where TRepo : IRepository<TEntity, int>
	{
		protected readonly TRepo Repo;

		public CrudService(TRepo repo)
		{
			Repo = repo;
		}

		public virtual async Task<ServiceResult> AddEntity<TAddModel>(TAddModel model)
		{
			var entity = new TEntity();

			foreach (var prop in model.GetType()
			                          .GetProperties())
			{
				// Disabled because TAddModel and TEntity should
				// have 100% parity (other than TEntity.Id), and
				// if this parity doesn't exist, we want to know
				// with an error
				// ReSharper disable once PossibleNullReferenceException
				entity.GetType()
				      .GetProperty(prop.Name)
				      .SetValue(entity, prop.GetValue(model));
			}

			try
			{
				await Repo.Create(entity);
			}
			catch (Exception e)
			{
				return ErrorHandler.HandleDbError(e);
			}
			
			return ServiceResult.Success;
		}

		public virtual async Task<ServiceResult> EditEntity<TEditModel>(TEditModel model)
		where TEditModel : IEntity<int>
		{
			TEntity entity;
			try
			{
				entity = await Repo.FindById(model.Id);
			}
			catch (Exception e)
			{
				return ErrorHandler.HandleDbError(e);
			}

			if (entity == null)
				return new ServiceResult($"A {typeof(TEntity).Name} with ID {model.Id} could not be found");
			
			foreach (var prop in model.GetType().GetProperties())
			{
				if (prop.Name == "Id")
					continue;

				// ReSharper disable once PossibleNullReferenceException
				entity.GetType()
				       .GetProperty(prop.Name)
				       .SetValue(entity, prop.GetValue(model));
			}

			try
			{
				await Repo.Update(entity);
			}
			catch (Exception e)
			{
				return ErrorHandler.HandleDbError(e);
			}

			return ServiceResult.Success;
		}

		public virtual Task<TEntity> GetEntity(int id) => Repo.FindById(id);

		public virtual Task<IList<TEntity>> GetEntities() => Repo.GetAll();
	}
}