using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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

				application.Use(
					async (context, next) => {
						if (!SetResponseWhenRestricted(context))
							await next.Invoke();
					}
				);

				application.UseDefaultFiles();
				application.UseStaticFiles();
			}

			bool SetResponseWhenRestricted(
				HttpContext context
			) {
				if (IsRestricted()) {
					if (context.User.Identity.IsAuthenticated) {
						context.Response.Headers.Add("Cache-Control", "no-store");
						return false;
					}
					else {
						context.Response.StatusCode = 403;
						return true;
					}
				}
				else
					return false;

				bool IsRestricted() =>
					context.Request.Path.Value.StartsWith("/restricted/");
			}
		}
	}
}