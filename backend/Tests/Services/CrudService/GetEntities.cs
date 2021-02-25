using System.Collections.Generic;
using System.Threading.Tasks;
using Core.DataModels.Characters;
using Repositories.Characters;
using Services;
using Services.Characters;
using Tests.Mocks;
using Xunit;

namespace Tests.Services.CrudService
{
	public class GetEntities
	{
		[Fact]
		public async Task ReturnsListofSpeciesIfNoIdPassed()
		{
			var speciesRepo = RepositoryMockFactory.SpeciesRepository();
			var service = new CrudService<Species, ISpeciesRepository>(speciesRepo.Object);

			var species = await service.GetEntities();
			
			Assert.NotNull(species);
			Assert.IsType<List<Species>>(species);
			Assert.Equal(2, species.Count);
		}
	}
}