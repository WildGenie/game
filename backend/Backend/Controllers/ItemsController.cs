using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Tools;
using Core.DataModels.Inventory;
using Core.ViewModels.Inventory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Inventory;

namespace Backend.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ItemsController : ControllerBase
	{
		readonly IItemService _service;

		public ItemsController(IItemService service)
		{
			_service = service;
		}

		[HttpGet]
		public async Task<IActionResult> GetItems()
		{
			var result = await _service.GetEntities();
			if (!result.WasSuccessful)
				return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse(result));

			var response = new ApiResponse<IList<Item>>
			{
				Result = result.Result
			};

			return Ok(response);
		}
		
		[HttpGet("{id}")]
		public async Task<IActionResult> GetItem(int id)
		{
			var result = await _service.GetEntity(id);

			if (!result.WasSuccessful)
				return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse(result));

			if (result.Result == null)
				return NotFound();

			return Ok(new ApiResponse<Item>(result.Result));
		}
		
		[HttpPost]
		public async Task<IActionResult> AddItem(AddItemModel item)
		{
			var result = await _service.AddEntity(item);
			if (!result.WasSuccessful)
				return UnprocessableEntity(new ApiResponse(result));

			return NoContent();
		}
		
		[HttpPatch]
		public async Task<IActionResult> EditItem(EditItemModel item)
		{
			var result = await _service.EditEntity(item);
			if (!result.WasSuccessful)
				return UnprocessableEntity(new ApiResponse(result));

			return NoContent();
		}
	}
}