using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Tools;
using Core.DataModels.Characters;
using Core.ViewModels.Characters.Species;
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
			var response = new ApiResponse<IList<Species>>
			{
				Result = await _speciesService.GetSpecies()
			};
			return Ok(response);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetSpecies(int id)
		{
			var species = await _speciesService.GetSpecies(id);
			if (species == null)
				return NotFound();

			return Ok(new ApiResponse<Species>(species));
		}

		[HttpPost]
		public async Task<IActionResult> AddSpecies(AddSpeciesModel species)
		{
			var result = await _speciesService.AddSpecies(species);
			if (!result.WasSuccessful)
				return UnprocessableEntity(new ApiResponse(result));

			return NoContent();
		}

		[HttpPatch]
		public async Task<IActionResult> EditSpecies(EditSpeciesModel species)
		{
			var result = await _speciesService.EditSpecies(species);
			if (!result.WasSuccessful)
				return UnprocessableEntity(new ApiResponse(result));

			return NoContent();
		}
	}
}