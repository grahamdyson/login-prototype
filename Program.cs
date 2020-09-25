using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Company.Users.LoginPrototype
{
	class Program
	{
		public static void Main(
			string[] args) =>
			Host.CreateDefaultBuilder(args)
			.ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
			.Build().Run();

		class Startup
		{
			public void Configure(
				IApplicationBuilder application,
				IWebHostEnvironment environment)
			{
				application.UseHttpsRedirection();
				
				application.UseDefaultFiles();
				application.UseStaticFiles();
			}
		}
	}
}