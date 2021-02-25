using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Core.DataModels.Inventory
{
	[ExcludeFromCodeCoverage]
	public class Item
	{
		public int Id { get; set; }
		
		[Required]
		public string Name { get; set; }
		
		[Required]
		[MaxLength(500)]
		public string Description { get; set; }
		public short MaxDurability { get; set; }
		public byte MaxCharges { get; set; }
		public ItemType Type { get; set; }
	}
}