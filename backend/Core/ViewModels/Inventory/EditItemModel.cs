using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Core.DataModels.Inventory;

namespace Core.ViewModels.Inventory
{
	[ExcludeFromCodeCoverage]
	public class EditItemModel : IEntity<int>
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