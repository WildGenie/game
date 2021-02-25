using System.Threading.Tasks;
using Core.DataModels.Characters;
using Core.ViewModels.Characters.Species;
using Repositories.Characters;
using Services;
using Services.Characters;
using Tests.Mocks;
using Xunit;

namespace Tests.Services.CrudService
{
	public class EditEntity
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
			var service = new CrudService<Species, ISpeciesRepository>(speciesRepo.Object);

			var result = await service.EditEntity(_species);

			Assert.False(result.WasSuccessful);
		}

		[Fact]
		public async Task ReturnsNullIfSpeciesNotExists()
		{
			var speciesRepo = RepositoryMockFactory.SpeciesRepository(successful: false);
			var service = new CrudService<Species,ISpeciesRepository>(speciesRepo.Object);

			var result = await service.EditEntity(_species);

			Assert.False(result.WasSuccessful);
			Assert.Equal($"A Species with ID {_species.Id} could not be found", result.Message);
		}

		[Fact]
		public async Task ReturnsServiceResultFailedIfUpdateFails()
		{
			var speciesRepo = RepositoryMockFactory.SpeciesRepository(updateThrows: true);
			var service = new CrudService<Species, ISpeciesRepository>(speciesRepo.Object);

			var result = await service.EditEntity(_species);
			
			Assert.False(result.WasSuccessful);
			Assert.Equal("A database error occurred. Please try again later.", result.Message);
		}

		[Fact]
		public async Task ReturnsSuccessIfUpdateSucceeds()
		{
			var speciesRepo = RepositoryMockFactory.SpeciesRepository();
			var service = new CrudService<Species, ISpeciesRepository>(speciesRepo.Object);

			var result = await service.EditEntity(_species);

			Assert.True(result.WasSuccessful);
		}
	}
}