using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BloggingSystem
{
	[Authorize]
	public class UsersController : BaseController
	{
		private readonly ILogger<UsersController> _logger;
		private readonly UserManager _userManager;

		public UsersController(ILogger<UsersController> logger, UserManager userManager, SubscribeManager subscribeManager)
			: base(subscribeManager)
		{
			_logger = logger;
			_userManager = userManager;
		}

		public async Task<IActionResult> AuthorDetailsAsync(string author)
		{
			if (author is null)
			{
				return NotFound();
			}

			var username = User.FindFirst(ClaimTypes.Name)?.Value;
			var subscriptions = await _subscribeManager.GetSubscriptionsAsync(username);

			ViewBag.Subscriptions = subscriptions;
			ViewBag.IsSubscribed = subscriptions.FirstOrDefault(sub => sub.Username.Equals(author)) is not null;
			await FillNotificationsAsync();

			return View("AuthorDetails", await _userManager.GetUserDetailsAsync(author));
		}

		public async Task<IActionResult> ProfileDetailsAsync(string author)
		{
			if (author is null)
			{
				return NotFound();
			}

			await FillSubscriptionsAsync();
			await FillNotificationsAsync();

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
				var message = "Failed to delete the post";
				_logger.LogError(ex, message);
				return Json(new
				{
					success = false,
					message
				});
			}
		}

		[HttpPost]
		public async Task<IActionResult> ToggleSubscriptionAsync(bool isSubscribed, string author)
		{
			try
			{
				var subscriber = User.FindFirst(ClaimTypes.Name)?.Value;

				if (isSubscribed)
				{
					await _subscribeManager.UnsubscribeAsync(author, subscriber);

					return Json(new
					{
						success = true
					});
				}

				return Json(new
				{
					success = true,
					subscription = await _subscribeManager.SubscribeAsync(author, subscriber)
				});
			}
			catch (Exception ex)
			{
				var message = $"Failed to {(isSubscribed ? "unsubscribe from" : "subscribe to")} {author}";
				_logger.LogError(ex, message);
				return Json(new
				{
					success = false,
					message
				});
			}
		}

		[HttpPost]
		public async Task<IActionResult> RemoveNotificationAsync(string subscriber, string notification)
		{
			try
			{
				await _subscribeManager.RemoveNotificationAsync(subscriber, notification);

				return Json(new
				{
					success = true
				});
			}
			catch (Exception ex)
			{
				var message = $"Failed to delete {notification} for {subscriber}";
				_logger.LogError(ex, message);
				return Json(new
				{
					success = false,
					message
				});
			}
		}
	}
}