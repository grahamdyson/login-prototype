using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
			public void ConfigureServices(
				IServiceCollection services)
			{
				services
				.AddAuthentication(
					CookieAuthenticationDefaults.AuthenticationScheme
				)
				.AddCookie(
					builder => {
						builder.Cookie.SameSite = SameSiteMode.Strict;
						builder.Cookie.SecurePolicy = CookieSecurePolicy.Always;
					});

				services
				.AddControllers();
			}

			public void Configure(
				IApplicationBuilder application,
				IWebHostEnvironment environment)
			{
				application.UseHttpsRedirection();
				application.UseAuthentication();
				
				application.UseRouting();
				application.UseEndpoints(endpoints => endpoints.MapControllers());

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
					else
					{
						RedirectToLogin();
						return true;
					}
				}
				else
					return false;

				bool IsRestricted() =>
					context.Request.Path.Value.StartsWith("/restricted/");

				void RedirectToLogin() =>
					context.Response.Redirect(
						QueryHelpers.AddQueryString(
							"/login/",
							new Dictionary<string, string>() { ["redirect"] = context.Request.Path.Value }));
			}
		}
	}
}