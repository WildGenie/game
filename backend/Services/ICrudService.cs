using System.Collections.Generic;
using System.Threading.Tasks;
using Core;

namespace Services
{
	public interface ICrudService<TModel> where TModel : new()
	{
		Task<ServiceResult> AddEntity<TAddModel>(TAddModel model);
		Task<ServiceResult> EditEntity<TEditModel>(TEditModel model)
			where TEditModel : IEntity<int>;
		Task<TModel> GetEntity(int id);
		Task<IList<TModel>> GetEntities();
	}
}