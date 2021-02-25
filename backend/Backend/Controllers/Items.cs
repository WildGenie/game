using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Tools;
using Core.DataModels.Inventory;
using Microsoft.AspNetCore.Mvc;
using Services.Inventory;

namespace Backend.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class Items : ControllerBase
	{
		readonly IItemService _service;

		public Items(IItemService service)
		{
			_service = service;
		}

		// [HttpGet]
		// public async Task<IActionResult> GetItems()
		// {
		// 	var response = new ApiResponse<IList<Item>>
		// 	{
		// 		Result = await _service.GetEntities()
		// 	};
		// 	return Ok(response);
		// }
	}
}