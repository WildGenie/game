using System.Diagnostics.CodeAnalysis;
using Core.DataModels.Inventory;
using Repositories.Inventory;

namespace Services.Inventory
{
	[ExcludeFromCodeCoverage]
	public class ItemService : CrudService<Item, IItemRepository>,
	                           IItemService
	{
		public ItemService(IItemRepository repo) : base(repo) { }
	}
}