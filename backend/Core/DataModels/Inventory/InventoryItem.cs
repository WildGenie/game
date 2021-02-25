using System.Diagnostics.CodeAnalysis;

namespace Core.DataModels.Inventory
{
	[ExcludeFromCodeCoverage]
	public class InventoryItem
	{
		public int Id { get; set; }
		public short Durability { get; set; }
		public byte Charges { get; set; }
		public int ItemId { get; set; }
		public virtual Item Item { get; set; }
		// public int CharacterId { get; set; }
		// public virtual Character Character { get; set; }
	}
}