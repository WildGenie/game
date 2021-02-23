using System.Threading.Tasks;
using Core.ViewModels.Characters.Species;
using Services.Characters;
using Tests.Mocks;
using Xunit;

namespace Tests.Services.Characters.SpeciesServiceTests
{
	public class EditSpecies
	{
		private EditSpeciesModel _species = new EditSpeciesModel
        {
            Id = 1,
            Name = "Hoo-mon",
            PluralName = "Hoo-mons",
            Description = "Hoo-mons have rejected the Second Rule of Acquisition",
            ForceSensitive = true,
            HpCoefficient = 10.0f
        };

		[Fact]
		public async Task ReturnsFailedIfDbThrows()
		{
			var speciesRepo = RepositoryMockFactory.SpeciesRepository(successful: false, throws: true);
			var service = new SpeciesService(speciesRepo.Object);

			var result = await service.EditSpecies(_species);

			Assert.False(result.WasSuccessful);
		}

		[Fact]
		public async Task ReturnsNullIfSpeciesNotExists()
		{
			var speciesRepo = RepositoryMockFactory.SpeciesRepository(successful: false);
			var service = new SpeciesService(speciesRepo.Object);

			var result = await service.EditSpecies(_species);

			Assert.False(result.WasSuccessful);
			Assert.Equal($"A species with ID {_species.Id} does not exist", result.Message);
		}

		[Fact]
		public async Task ReturnsServiceResultFailedIfUpdateFails()
		{
			var speciesRepo = RepositoryMockFactory.SpeciesRepository(updateThrows: true);
			var service = new SpeciesService(speciesRepo.Object);

			var result = await service.EditSpecies(_species);
			
			Assert.False(result.WasSuccessful);
			Assert.Equal("A database error occurred. Please try again later.", result.Message);
		}

		[Fact]
		public async Task ReturnsSuccessIfUpdateSucceeds()
		{
			var speciesRepo = RepositoryMockFactory.SpeciesRepository();
			var service = new SpeciesService(speciesRepo.Object);

			var result = await service.EditSpecies(_species);

			Assert.True(result.WasSuccessful);
		}
	}
}