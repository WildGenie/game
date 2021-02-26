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
	public class GetItem
	{
		[Fact]
		public async Task ReturnsNotFoundResultIfItemNotExists()
		{
			var service = ServiceMockFactory.ItemService(successful: false);
			var controller = new ItemsController(service.Object);

			var response = await controller.GetItem(1);

			Assert.IsType<NotFoundResult>(response);
		}

		[Fact]
		public async Task ReturnsItemIfItemExists()
		{
			var service = ServiceMockFactory.ItemService();
			var controller = new ItemsController(service.Object);

			var response = await controller.GetItem(1);

			Assert.IsType<OkObjectResult>(response);

			var result = (response as OkObjectResult)?.Value as ApiResponse<Item> ?? new ApiResponse<Item>();
			
			Assert.NotNull(result);
			Assert.NotNull(result.Result);
			Assert.Equal("SomeItem", result.Result.Name);
		}

		[Fact]
		public async Task ReturnsObjectResultIfDbThrows()
		{
			var service = ServiceMockFactory.ItemService(successful: false, throws: true);
			var controller = new ItemsController(service.Object);

			var response = await controller.GetItem(1);

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