using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Company.Users.LoginPrototype
{
	[ApiController]
	[Route("authentication")]
	public class AuthenticationApiController : ControllerBase
	{
		public class Credentials
		{
			[Required]
			public string Email { get; set; }

			[Required]
			public string Password { get; set; }
		}

		[Consumes("application/json")]
		[HttpPost]
		public async Task<IActionResult> Authenticate(
			Credentials credentials)
		{
			if (AreCredentialsValid())
			{
				await SignInWithEmail(credentials.Email);

				return Ok();
			}
			else
				return Unauthorized();

			bool AreCredentialsValid() {
				return
					credentials.Email == "testuser@domain.com"
					&&
					credentials.Password == "dontstoreplaintextpasswords";
			}
		}

		Task SignInWithEmail(
			string email) =>
			HttpContext.SignInAsync(
				new ClaimsPrincipal(
					new ClaimsIdentity(
						new[] { new Claim(ClaimTypes.Name, email) },
						CookieAuthenticationDefaults.AuthenticationScheme)));
	}
}