using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Repositories
{
	[ExcludeFromCodeCoverage]
	public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
	{
		public ApplicationDbContext CreateDbContext(string[] args)
		{
			var currentDirectory = Directory.GetCurrentDirectory();
			var appsettings = currentDirectory + "/../Backend/appsettings.Development.json";
			IConfiguration config = new ConfigurationBuilder()
									.SetBasePath(currentDirectory)
									.AddJsonFile(appsettings)
									.Build();

			var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
			var connectionString = config.GetConnectionString("Default");
			if (string.IsNullOrEmpty(connectionString))
			{
				throw new NullReferenceException("Database connection string cannot be null.");
			}

			builder.UseLazyLoadingProxies()
				   .UseMySql(connectionString);

			return new ApplicationDbContext(builder.Options);
		}
	}
}