using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BloggingSystem
{
	public class AuthController : Controller
	{
		private readonly UserService _userService;
		private readonly ILogger<AuthController> _logger;

		public AuthController(UserService userService, ILogger<AuthController> logger)
		{
			_userService = userService;
			_logger = logger;
		}

		public IActionResult Login(string returnUrl = null)
		{
			ViewData["ReturnUrl"] = returnUrl;
			return View();
		}

		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> LoginAsync(LoginCredentials credentials, string returnUrl = null)
		{
			try
			{
				var user = await _userService.AuthenticateUserAsync(credentials);

				var claims = new List<Claim>
				{
					new Claim(ClaimTypes.Name, user.Username),
					// Добавьте другие утверждения, если необходимо
				};

				var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

				await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
					new ClaimsPrincipal(claimsIdentity));

				if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
				{
					return Redirect(returnUrl);
				}

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
				var user = await _userService.RegisterUserAsync(credentials);
				return RedirectToAction("Login", "Auth");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Registration failed");
				return BadRequest(ex.Message);
			}
		}

		[HttpPost("logout")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> LogoutAsync()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Login", "Auth");
		}
	}
}