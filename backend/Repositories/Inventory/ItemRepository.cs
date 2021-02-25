using System.Diagnostics.CodeAnalysis;
using Core.DataModels.Inventory;

namespace Repositories.Inventory
{
	[ExcludeFromCodeCoverage]
	public class ItemRepository : Repository<Item, int>,
	                              IItemRepository
	{
		public ItemRepository(ApplicationDbContext context) : base(context)
		{
		}
	}
}