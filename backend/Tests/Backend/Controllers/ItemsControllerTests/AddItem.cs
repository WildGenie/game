using System.Threading.Tasks;
using Backend.Controllers;
using Backend.Tools;
using Core.ViewModels.Characters.Species;
using Core.ViewModels.Inventory;
using Microsoft.AspNetCore.Mvc;
using Tests.Mocks;
using Xunit;

namespace Tests.Backend.Controllers.ItemsControllerTests
{
	public class AddItem
	{
		[Fact]
		public async Task ReturnsBadRequestIfAddSpeciesFails()
		{
			var service = ServiceMockFactory.ItemService(successful: false, throws: true);
			var controller = new ItemsController(service.Object);

			var response = await controller.AddItem(new AddItemModel());

			Assert.IsType<UnprocessableEntityObjectResult>(response);

			var result = (response as UnprocessableEntityObjectResult)?.Value as ApiResponse;

			Assert.NotNull(result);
			Assert.Contains("General", result.Errors.Keys);
			Assert.Single(result.Errors["General"]);
			Assert.Equal("Mock this bad service result", result.Errors["General"][0]);
		}

		[Fact]
		public async Task ReturnsNoContentResultIfAddSpeciesSucceeds()
		{
			var service = ServiceMockFactory.ItemService();
			var controller = new ItemsController(service.Object);

			var response = await controller.AddItem(new AddItemModel());

			Assert.IsType<NoContentResult>(response);
		}
	}
}