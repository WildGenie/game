using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Controllers;
using Backend.Tools;
using Core;
using Core.DataModels.Characters;
using Core.DataModels.Inventory;
using Microsoft.AspNetCore.Mvc;
using Tests.Mocks;
using Xunit;

namespace Tests.Backend.Controllers.ItemsControllerTests
{
	public class GetAllSpecies
	{
		[Fact]
		public async Task ReturnsOkObjectResult()
		{
			var service = ServiceMockFactory.ItemService();
			var controller = new ItemsController(service.Object);

			var response = await controller.GetItems();
			
			Assert.NotNull(response);
			Assert.IsType<OkObjectResult>(response);
		}

		[Fact]
		public async Task ReturnsListOfItems()
		{
			var service = ServiceMockFactory.ItemService();
			var controller = new ItemsController(service.Object);

			var response = await controller.GetItems();
			var result = (response as OkObjectResult)?.Value as ApiResponse<IList<Item>> ?? new ApiResponse<IList<Item>>();
			
			Assert.NotEmpty(result.Result);
			Assert.Single(result.Result);
			Assert.Equal("SomeItem", result.Result[0].Name);
		}

		[Fact]
		public async Task ReturnsObjectResultIfLookupThrows()
		{
			var service = ServiceMockFactory.ItemService(successful: false, throws: true);
			var controller = new ItemsController(service.Object);

			var response = await controller.GetItems();
			Assert.IsType<ObjectResult>(response);
			
			var result = (response as ObjectResult)?.Value as ApiResponse ?? new ApiResponse(new ServiceResult());
			
			Assert.NotEmpty(result.Errors);
			Assert.Contains("General", result.Errors.Keys);
			Assert.Single(result.Errors["General"]);
			Assert.Equal("Mock this bad service result", result.Errors["General"][0]);
		}
	}
}