using System.Threading.Tasks;
using Serilog;

namespace Backend.Tools.Seeders
{
	public abstract class DatabaseSeeder<TManager>
	{
		protected readonly TManager Service;
		protected readonly ILogger Logger;

		protected DatabaseSeeder(TManager service, ILogger logger)
		{
			Service = service;
			Logger = logger;
		}

		public abstract Task SeedData();
	}
}