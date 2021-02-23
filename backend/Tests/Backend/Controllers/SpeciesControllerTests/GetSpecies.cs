using System.Threading.Tasks;
using Backend.Controllers;
using Backend.Tools;
using Core.DataModels.Characters;
using Microsoft.AspNetCore.Mvc;
using Tests.Mocks;
using Xunit;

namespace Tests.Backend.Controllers.SpeciesControllerTests
{
	public class GetSpecies
	{
		[Fact]
		public async Task ReturnsNotFoundResultIfSpeciesNotExists()
		{
			var speciesService = ServiceMockFactory.SpeciesService(successful: false);
			var controller = new SpeciesController(speciesService.Object);

			var response = await controller.GetSpecies(1);

			Assert.IsType<NotFoundResult>(response);
		}

		[Fact]
		public async Task ReturnsSpeciesIfSpeciesExists()
		{
			var speciesService = ServiceMockFactory.SpeciesService();
			var controller = new SpeciesController(speciesService.Object);

			var response = await controller.GetSpecies(1);

			Assert.IsType<OkObjectResult>(response);

			var result = (response as OkObjectResult)?.Value as ApiResponse<Species> ?? new ApiResponse<Species>();
			
			Assert.NotNull(result);
			Assert.NotNull(result.Result);
			Assert.Equal("Human", result.Result.Name);
		}
	}
}