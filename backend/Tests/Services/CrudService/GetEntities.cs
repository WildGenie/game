using System.Collections.Generic;
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
	public class GetEntities
	{
		[Fact]
		public async Task ReturnsListofSpeciesIfNoIdPassed()
		{
			var speciesRepo = RepositoryMockFactory.SpeciesRepository();
			var service = new CrudService<Species, ISpeciesRepository>(speciesRepo.Object);

			var result = await service.GetEntities();
			
			Assert.NotNull(result);
			Assert.IsType<ServiceResult<IList<Species>>>(result);

			var species = result.Result;
			Assert.NotNull(species);
			Assert.Equal(2, species.Count);
		}

		[Fact]
		public async Task ReturnsErrorMessageServiceResultIfDatabaseFailed()
		{
			var speciesRepo = RepositoryMockFactory.SpeciesRepository(successful: false);
			var service = new CrudService<Species, ISpeciesRepository>(speciesRepo.Object);

			var result = await service.GetEntities();
			
			Assert.NotNull(result);
			Assert.False(result.WasSuccessful);
			Assert.Equal("A database error occurred. Please try again later.", result.Message);
		}
	}
}