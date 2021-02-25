using System.Threading.Tasks;
using Backend.Controllers;
using Backend.Tools;
using Core;
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

		[Fact]
		public async Task ReturnsObjectResultIfDbThrows()
		{
			var speciesService = ServiceMockFactory.SpeciesService(successful: false, throws: true);
			var controller = new SpeciesController(speciesService.Object);

			var response = await controller.GetSpecies(1);

			Assert.IsType<ObjectResult>(response);

			var result = (response as ObjectResult)?.Value as ApiResponse ?? new ApiResponse(new ServiceResult());
			
			Assert.NotNull(result);
			Assert.NotEmpty(result.Errors);
			Assert.Contains("General", result.Errors.Keys);
			Assert.Single(result.Errors["General"]);
			Assert.Equal("Mock this bad service result", result.Errors["General"][0]);
		}
	}
}