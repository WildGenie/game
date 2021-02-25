using System.Threading.Tasks;
using Core.DataModels.Characters;
using Repositories.Characters;
using Services;
using Services.Characters;
using Tests.Mocks;
using Xunit;

namespace Tests.Services.CrudService
{
	public class GetEntity
	{
		[Fact]
		public async Task ReturnsSpeciesIfIdExists()
		{
			var speciesRepo = RepositoryMockFactory.SpeciesRepository();
			var service = new CrudService<Species, ISpeciesRepository>(speciesRepo.Object);

			var species = await service.GetEntity(1);

			Assert.NotNull(species);
			Assert.IsType<Species>(species);
			Assert.Equal("Human", species.Name);
		}

		[Fact]
		public async Task ReturnsNullIfIdNotExists()
		{
			var speciesRepo = RepositoryMockFactory.SpeciesRepository(successful: false);
			var service = new CrudService<Species, ISpeciesRepository>(speciesRepo.Object);

			var species = await service.GetEntity(1);

			Assert.Null(species);
		}
	}
}