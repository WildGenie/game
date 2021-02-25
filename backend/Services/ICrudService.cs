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
		Task<ServiceResult<TModel>> GetEntity(int id);
		Task<ServiceResult<IList<TModel>>> GetEntities();
	}
}