using System.Diagnostics.CodeAnalysis;
using Core.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Configurations
{
	[ExcludeFromCodeCoverage]
	public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
	{
		public void Configure(EntityTypeBuilder<ApplicationUser> builder)
		{
			builder.HasMany(u => u.UserRoles)
				   .WithOne()
				   .HasForeignKey(ur => ur.UserId)
				   .IsRequired();
		}
	}
}