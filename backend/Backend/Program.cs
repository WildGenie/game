using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace Backend
{
	// Excluded from coverage
	// Program can't be unit tested because it doesn't represent a unit
	// It represents the entire application
	[ExcludeFromCodeCoverage]
	public class Program
	{
		public static void Main(string[] args)
		{
			var path = $"{Directory.GetCurrentDirectory()}/log";
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}

			Log.Logger = new LoggerConfiguration()
						 .MinimumLevel.Debug()
						 .WriteTo.File($"{path}/log-.log", rollingInterval: RollingInterval.Hour, restrictedToMinimumLevel: LogEventLevel.Error, retainedFileCountLimit: 168)
						 .WriteTo.Console()
						 .CreateLogger();

			try
			{
				Log.Information("Starting backend API");
				CreateHostBuilder(args)
					.Build()
					.Run();
			}
			catch (Exception e)
			{
				Log.Fatal(e, "Backend API terminated unexpectedly");
			}
			finally
			{
				Log.Information("Shutting down backend API");
				Log.CloseAndFlush();
			}
		}

		public static IHostBuilder CreateHostBuilder(string[] args)
		{
			return Host.CreateDefaultBuilder(args)
					   .UseSerilog()
					   .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
		}
	}
}