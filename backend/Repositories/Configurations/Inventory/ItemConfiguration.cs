using Core.DataModels.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Configurations.Inventory
{
	public class ItemConfiguration : IEntityTypeConfiguration<Item>
	{
		public void Configure(EntityTypeBuilder<Item> builder)
		{
			builder.HasIndex(i => i.Name)
				   .IsUnique();
		}
	}
}