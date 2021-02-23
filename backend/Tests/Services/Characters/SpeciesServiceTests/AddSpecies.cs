using System.Threading.Tasks;
using Core.DataModels.Characters;
using Core.ViewModels.Characters.Species;
using Services.Characters;
using Tests.Mocks;
using Xunit;

namespace Tests.Services.Characters.SpeciesServiceTests
{
	public class AddSpecies
	{
		[Fact]
		public async Task ReturnsServiceResultSuccessIfSuccessful()
		{
			var speciesRepo = RepositoryMockFactory.SpeciesRepository();
			var service = new SpeciesService(speciesRepo.Object);

			var species = new AddSpeciesModel
			{
				Name = "Human",
				PluralName = "Humans",
				Description = "Humans are good and cool",
				ForceSensitive = true,
				HpCoefficient = 10.0f
			};
			
			var result = await service.AddSpecies(species);

			Assert.True(result.WasSuccessful);
		}

		[Fact]
		public async Task ReturnsServiceResultFailedIfFailed()
		{
			var speciesRepo = RepositoryMockFactory.SpeciesRepository(successful: false);
			var service = new SpeciesService(speciesRepo.Object);

			var species = new AddSpeciesModel
			{
				Name = "Human",
				PluralName = "Humans",
				Description = "Humans are good and cool",
				ForceSensitive = true,
				HpCoefficient = 10.0f
			};
			
			var result = await service.AddSpecies(species);

			Assert.False(result.WasSuccessful);
		}
	}
}