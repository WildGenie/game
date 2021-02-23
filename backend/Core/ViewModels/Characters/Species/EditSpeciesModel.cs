using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Core.ViewModels.Characters.Species
{
	[ExcludeFromCodeCoverage]
	public class EditSpeciesModel
	{
		[Required]
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		[Required]
		public string PluralName { get; set; }

		[Required]
		[MaxLength(500)]
		public string Description { get; set; }
		public bool ForceSensitive { get; set; }
		public float HpCoefficient { get; set; }

		public short StrengthModifier { get; set; }
		public short DexterityModifier { get; set; }
		public short ConstitutionModifier { get; set; }
		public short IntelligenceModifier { get; set; }
		public short CharismaModifier { get; set; }
		public short WisdomModifier { get; set; }
		public short AwarenessModifier { get; set; }
	}
}