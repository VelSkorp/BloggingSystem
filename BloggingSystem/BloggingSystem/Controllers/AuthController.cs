﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BloggingSystemRepository;

namespace BloggingSystem
{
	public class AuthController : Controller
	{
		private readonly IUserRepository _userRepository;
		private readonly ILogger<AuthController> _logger;

		public AuthController(IUserRepository userRepository, ILogger<AuthController> logger)
		{
			_userRepository = userRepository;
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
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> LoginAsync(LoginCredentials credentials, string returnUrl = null)
		{
			try
			{
				var user = await _userRepository.AuthenticateUserAsync(credentials);

				var claims = new List<Claim>
				{
					new Claim(ClaimTypes.Name, user.Username),
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
				var user = await _userRepository.RegisterUserAsync(credentials);
				return RedirectToAction("Login", "Auth");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Registration failed");
				return BadRequest(ex.Message);
			}
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> LogoutAsync()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Login", "Auth");
		}
	}
}