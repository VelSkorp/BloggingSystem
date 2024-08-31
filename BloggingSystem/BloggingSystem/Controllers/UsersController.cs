using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BloggingSystem
{
	[Authorize]
	public class UsersController : Controller
	{
		private readonly ILogger<UsersController> _logger;
		private readonly UserManager _userManager;
		private readonly SubscribeManager _subscribeManager;

		public UsersController(ILogger<UsersController> logger, UserManager userManager, SubscribeManager subscribeManager)
		{
			_logger = logger;
			_userManager = userManager;
			_subscribeManager = subscribeManager;
		}

		public async Task<IActionResult> AuthorDetailsAsync(string author)
		{
			if (author is null)
			{
				return NotFound();
			}

			return View("AuthorDetails", await _userManager.GetUserDetailsAsync(author));
		}

		public async Task<IActionResult> ProfileDetailsAsync(string author)
		{
			if (author is null)
			{
				return NotFound();
			}

			return View("ProfileDetails", await _userManager.GetUserDetailsAsync(author));
		}

		[HttpPost]
		public async Task<IActionResult> UpdateUserAsync(string firstName, string lastName, IFormFile photo)
		{
			try
			{
				var username = User.FindFirst(ClaimTypes.Name)?.Value;
				var photoUrl = await _userManager.UpdateUserDetailsAsync(username, firstName, lastName, photo);

				return Json(new
				{
					success = true,
					photo = photoUrl,
					firstName,
					lastName
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to delete the post");
				return RedirectToAction("Error", "Posts");
			}
		}

		[HttpPost]
		public async Task<IActionResult> SubscribeAsync(string author, string subscriber)
		{
			await _subscribeManager.SubscribeAsync(author, subscriber);
			return Ok();
		}

		[HttpPost]
		public async Task<IActionResult> UnsubscribeAsync(string author, string subscriber)
		{
			await _subscribeManager.UnsubscribeAsync(author, subscriber);
			return Ok();
		}

		[HttpPost]
		public async Task<IActionResult> RemoveNotificationAsync(string subscriber, string notification)
		{
			await _subscribeManager.RemoveNotificationAsync(subscriber, notification);
			return Ok();
		}
	}
}