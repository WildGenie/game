using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Controllers;
using Backend.Tools;
using Core.DataModels.Characters;
using Microsoft.AspNetCore.Mvc;
using Tests.Mocks;
using Xunit;

namespace Tests.Backend.Controllers.SpeciesControllerTests
{
	public class GetAllSpecies
	{
		[Fact]
		public async Task ReturnsOkObjectResult()
		{
			var speciesService = ServiceMockFactory.SpeciesService();
			var controller = new SpeciesController(speciesService.Object);

			var response = await controller.GetAllSpecies();
			
			Assert.NotNull(response);
			Assert.IsType<OkObjectResult>(response);
		}

		[Fact]
		public async Task ReturnsListOfSpecies()
		{
			var speciesService = ServiceMockFactory.SpeciesService();
			var controller = new SpeciesController(speciesService.Object);

			var response = await controller.GetAllSpecies();
			var result = (response as OkObjectResult)?.Value as ApiResponse<IList<Species>> ?? new ApiResponse<IList<Species>>();
			
			Assert.NotEmpty(result.Result);
			Assert.Single(result.Result);
			Assert.Equal("Human", result.Result[0].Name);
		}
	}
}