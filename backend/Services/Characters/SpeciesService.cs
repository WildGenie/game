using System.Diagnostics.CodeAnalysis;
using Core.DataModels.Characters;
using Repositories.Characters;

namespace Services.Characters
{
	[ExcludeFromCodeCoverage]
	public class SpeciesService : CrudService<Species, ISpeciesRepository>, ISpeciesService
	{
		public SpeciesService(ISpeciesRepository repo) : base(repo) { }
	}
}