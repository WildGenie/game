using System.Threading.Tasks;
using Core;
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

			var result = await service.GetEntity(1);

			Assert.NotNull(result);
			Assert.IsType<ServiceResult<Species>>(result);

			var species = result.Result;
			Assert.NotNull(species);
			Assert.Equal("Human", species.Name);
		}

		[Fact]
		public async Task ReturnsNullIfIdNotExists()
		{
			var speciesRepo = RepositoryMockFactory.SpeciesRepository(successful: false);
			var service = new CrudService<Species, ISpeciesRepository>(speciesRepo.Object);

			var result = await service.GetEntity(1);
			
			Assert.NotNull(result);
			Assert.IsType<ServiceResult<Species>>(result);
		}

		[Fact]
		public async Task ReturnsErrorServiceResultIfDbThrows()
		{
			var speciesRepo = RepositoryMockFactory.SpeciesRepository(throws: true);
			var service = new CrudService<Species, ISpeciesRepository>(speciesRepo.Object);

			var result = await service.GetEntity(1);
			
			Assert.NotNull(result);
			Assert.IsType<ServiceResult<Species>>(result);
			Assert.False(result.WasSuccessful);
			Assert.Equal("A database error occurred. Please try again later.", result.Message);
		}
	}
}