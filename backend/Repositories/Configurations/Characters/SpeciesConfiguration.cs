using Core.DataModels.Characters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Configurations.Characters
{
	public class SpeciesConfiguration : IEntityTypeConfiguration<Species>
	{
		public void Configure(EntityTypeBuilder<Species> builder)
		{
			builder.HasIndex(s => s.Name)
				   .IsUnique();

			builder.HasIndex(s => s.PluralName)
				   .IsUnique();
		}
	}
}