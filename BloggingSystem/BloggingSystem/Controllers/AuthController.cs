using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BloggingSystemRepository;

namespace BloggingSystem
{
	public class AuthController : Controller
	{
		private readonly ILogger<AuthController> _logger;
		private readonly UserManager _userManager;

		public AuthController(UserManager userManager, ILogger<AuthController> logger)
		{
			_userManager = userManager;
			_logger = logger;
		}

		public IActionResult Login()
		{
			if (User.Identity.IsAuthenticated)
			{
				return RedirectToAction("Index", "Posts");
			}
			return View();
		}

		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> LoginAsync(LoginCredentials credentials)
		{
			try
			{
				var user = await _userManager.AuthenticateUserAsync(credentials);
				await SaveUserCookieAsync(user);
				return RedirectToAction("Index", "Posts");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Login failed");
				ViewBag.ErrorMessage = ex.Message;
				return View();
			}
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> RegisterAsync(RegisterCredentials credentials)
		{
			try
			{
				var user = await _userManager.RegisterUserAsync(credentials);
				await SaveUserCookieAsync(user);
				return RedirectToAction("Index", "Posts");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Registration failed");
				return BadRequest(ex.Message);
			}
		}

		public async Task<IActionResult> LogoutAsync()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Login", "Auth");
		}

		private async Task SaveUserCookieAsync((string username, string photoUrl) user)
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, user.username),
				new Claim(ClaimTypes.UserData, user.photoUrl)
			};

			var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

			await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
				new ClaimsPrincipal(claimsIdentity));
		}
	}
}