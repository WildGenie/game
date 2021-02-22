using System.Diagnostics.CodeAnalysis;
using Core.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Configurations
{
	[ExcludeFromCodeCoverage]
	public class RoleConfiguration : IEntityTypeConfiguration<Role>
	{
		public void Configure(EntityTypeBuilder<Role> builder)
		{
			builder.HasMany(r => r.UserRoles)
				   .WithOne()
				   .HasForeignKey(ur => ur.RoleId)
				   .IsRequired();
		}
	}
}