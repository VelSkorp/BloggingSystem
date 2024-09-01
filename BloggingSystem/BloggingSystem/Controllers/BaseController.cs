using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BloggingSystem
{
	public abstract class BaseController : Controller
	{
		protected readonly SubscribeManager _subscribeManager;

		public BaseController(SubscribeManager subscribeManager)
		{
			_subscribeManager = subscribeManager;
		}

		public async Task FillSubscriptionsAsync()
		{
			var username = User.FindFirst(ClaimTypes.Name)?.Value;
			ViewBag.Subscriptions = await _subscribeManager.GetSubscriptionsAsync(username);
		}
	}
}