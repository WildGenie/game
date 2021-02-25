using System.Collections.Generic;
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

		[Fact]
		public async Task ReturnsObjectResultIfLookupThrows()
		{
			var speciesService = ServiceMockFactory.SpeciesService(successful: false, throws: true);
			var controller = new SpeciesController(speciesService.Object);

			var response = await controller.GetAllSpecies();
			Assert.IsType<ObjectResult>(response);
			
			var result = (response as ObjectResult)?.Value as ApiResponse ?? new ApiResponse(new ServiceResult());
			
			Assert.NotEmpty(result.Errors);
			Assert.Contains("General", result.Errors.Keys);
			Assert.Single(result.Errors["General"]);
			Assert.Equal("Mock this bad service result", result.Errors["General"][0]);
		}
	}
}