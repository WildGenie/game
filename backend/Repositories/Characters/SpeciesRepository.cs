using System.Diagnostics.CodeAnalysis;
using Core.DataModels.Characters;

namespace Repositories.Characters
{
	[ExcludeFromCodeCoverage]
	public class SpeciesRepository : Repository<Species, int>, ISpeciesRepository
	{
		public SpeciesRepository(ApplicationDbContext context) : base(context) { }
	}
}