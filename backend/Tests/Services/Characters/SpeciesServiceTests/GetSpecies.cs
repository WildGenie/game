using System.Collections.Generic;
using System.Threading.Tasks;
using Core.DataModels.Characters;
using Services.Characters;
using Tests.Mocks;
using Xunit;

namespace Tests.Services.Characters.SpeciesServiceTests
{
	public class GetSpecies
	{
		[Fact]
		public async Task ReturnsSpeciesIfIdExists()
		{
			var speciesRepo = RepositoryMockFactory.SpeciesRepository();
			var service = new SpeciesService(speciesRepo.Object);

			var species = await service.GetSpecies(1);

			Assert.NotNull(species);
			Assert.IsType<Species>(species);
			Assert.Equal("Human", species.Name);
		}

		[Fact]
		public async Task ReturnsNullIfIdNotExists()
		{
			var speciesRepo = RepositoryMockFactory.SpeciesRepository(successful: false);
			var service = new SpeciesService(speciesRepo.Object);

			var species = await service.GetSpecies(1);

			Assert.Null(species);
		}

		[Fact]
		public async Task ReturnsListofSpeciesIfNoIdPassed()
		{
			var speciesRepo = RepositoryMockFactory.SpeciesRepository();
			var service = new SpeciesService(speciesRepo.Object);

			var species = await service.GetSpecies();
			
			Assert.NotNull(species);
			Assert.IsType<List<Species>>(species);
			Assert.Equal(2, species.Count);
		}
}
}