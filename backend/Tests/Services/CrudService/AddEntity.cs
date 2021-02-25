using System.Threading.Tasks;
using Core.DataModels.Characters;
using Core.ViewModels.Characters.Species;
using Repositories.Characters;
using Services;
using Tests.Mocks;
using Xunit;

namespace Tests.Services.CrudService
{
	public class AddEntity
	{
		[Fact]
		public async Task ReturnsServiceResultSuccessIfSuccessful()
		{
			var speciesRepo = RepositoryMockFactory.SpeciesRepository();
			var service = new CrudService<Species,ISpeciesRepository>(speciesRepo.Object);

			var species = new AddSpeciesModel
			{
				Name = "Human",
				PluralName = "Humans",
				Description = "Humans are good and cool",
				ForceSensitive = true,
				HpCoefficient = 10.0f
			};
			
			var result = await service.AddEntity(species);

			Assert.True(result.WasSuccessful);
		}
	}
}