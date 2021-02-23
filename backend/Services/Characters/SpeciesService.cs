using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using Core.DataModels.Characters;
using Core.ViewModels.Characters.Species;
using Repositories.Characters;

namespace Services.Characters
{
	public class SpeciesService : ISpeciesService
	{
		private readonly ISpeciesRepository _repo;

		public SpeciesService(ISpeciesRepository repo)
		{
			_repo = repo;
		}

		public async Task<ServiceResult> AddSpecies(AddSpeciesModel model)
		{
			var species = new Species
			{
				Name = model.Name,
				PluralName = model.PluralName,
				Description = model.Description,
				ForceSensitive = model.ForceSensitive,
				HpCoefficient = model.HpCoefficient,
				StrengthModifier = model.StrengthModifier,
				DexterityModifier = model.DexterityModifier,
				ConstitutionModifier = model.ConstitutionModifier,
				IntelligenceModifier = model.IntelligenceModifier,
				CharismaModifier = model.CharismaModifier,
				WisdomModifier = model.WisdomModifier,
				AwarenessModifier = model.AwarenessModifier
			};
			try
			{
				await _repo.Create(species);
			}
			catch (Exception e)
			{
				return ErrorHandler.HandleDbError(e);
			}

			return ServiceResult.Success;
		}

		public async Task<ServiceResult> EditSpecies(EditSpeciesModel model)
		{
			Species species;
			try
			{
				species = await _repo.FindById(model.Id);
			}
			catch (Exception e)
			{
				return ErrorHandler.HandleDbError(e);
			}

			if (species == null)
				return new ServiceResult($"A species with ID {model.Id} does not exist");

			foreach (var prop in model.GetType().GetProperties())
			{
				if (prop.Name == "Id")
					continue;

				// Next line disabled because while there is a hypothetical
				// NullReferenceException risk, there is 100% object parity
				// between Species and EditSpeciesModel, so the risk
				// is fully mitigated unless EditSpeciesModel takes on
				// new properties
				// ReSharper disable once PossibleNullReferenceException
				species.GetType()
					   .GetProperty(prop.Name)
					   .SetValue(species, prop.GetValue(model));
			}

			try
			{
				await _repo.Update(species);
			}
			catch (Exception e)
			{
				return ErrorHandler.HandleDbError(e);
			}

			return ServiceResult.Success;
		}

		public async Task<Species> GetSpecies(int id) => await _repo.FindById(id);

		public async Task<IList<Species>> GetSpecies() => await _repo.GetAll();
	}
}