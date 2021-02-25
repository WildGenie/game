using System.Diagnostics.CodeAnalysis;
using Core.DataModels;
using Core.DataModels.Characters;
using Core.DataModels.Inventory;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Repositories.Configurations;
using Repositories.Configurations.Characters;
using Repositories.Configurations.Inventory;

namespace Repositories
{
	[ExcludeFromCodeCoverage]
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser, Role, string, IdentityUserClaim<string>, UserRole, IdentityUserLogin<string>,
		RoleClaim, IdentityUserToken<string>>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.ApplyConfiguration(new UserConfiguration());
			modelBuilder.ApplyConfiguration(new RoleConfiguration());
			modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
			modelBuilder.ApplyConfiguration(new SpeciesConfiguration());
			modelBuilder.ApplyConfiguration(new ItemConfiguration());
		}
		
		public DbSet<Species> Species { get; set; }
		public DbSet<Item> Items { get; set; }
	}
}