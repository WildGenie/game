using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Tools;
using Core.DataModels.Characters;
using Core.ViewModels.Characters.Species;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Characters;

namespace Backend.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class SpeciesController : ControllerBase
	{
		private readonly ISpeciesService _speciesService;

		public SpeciesController(ISpeciesService speciesService)
		{
			_speciesService = speciesService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllSpecies()
		{
			var result = await _speciesService.GetEntities();
			if (!result.WasSuccessful)
				return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse(result));
			
			var response = new ApiResponse<IList<Species>>
			{
				Result = result.Result
			};
			return Ok(response);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetSpecies(int id)
		{
			var result = await _speciesService.GetEntity(id);
			
			if (!result.WasSuccessful)
				return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse(result));
			
			if (result.Result == null)
				return NotFound();

			return Ok(new ApiResponse<Species>(result.Result));
		}

		[HttpPost]
		public async Task<IActionResult> AddSpecies(AddSpeciesModel species)
		{
			var result = await _speciesService.AddEntity(species);
			if (!result.WasSuccessful)
				return UnprocessableEntity(new ApiResponse(result));

			return NoContent();
		}

		[HttpPatch]
		public async Task<IActionResult> EditSpecies(EditSpeciesModel species)
		{
			var result = await _speciesService.EditEntity(species);
			if (!result.WasSuccessful)
				return UnprocessableEntity(new ApiResponse(result));

			return NoContent();
		}
	}
}