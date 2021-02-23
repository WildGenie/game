using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using Core.DataModels.Characters;
using Core.ViewModels.Characters.Species;

namespace Services.Characters
{
	public interface ISpeciesService
	{
		Task<ServiceResult> AddSpecies(AddSpeciesModel model);
		Task<ServiceResult> EditSpecies(EditSpeciesModel model);
		Task<Species> GetSpecies(int id);
		Task<IList<Species>> GetSpecies();
	}
}