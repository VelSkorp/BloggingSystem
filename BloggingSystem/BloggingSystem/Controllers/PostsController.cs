using BloggingSystemRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BloggingSystem
{
	[Authorize]
	public class PostsController : BaseController
	{
		private readonly ILogger<PostsController> _logger;
		private readonly PostManager _postManager;

		public PostsController(ILogger<PostsController> logger, SubscribeManager subscribeManager, PostManager postManager)
			: base(subscribeManager)
		{
			_logger = logger;
			_postManager = postManager;
		}

		public async Task<IActionResult> IndexAsync(string author)
		{
			await FillSubscriptionsAsync();
			await FillNotificationsAsync();
			return View("Index", await _postManager.GetPostsAsync(author));
		}

		public async Task<IActionResult> CreateAsync()
		{
			await FillSubscriptionsAsync();
			await FillNotificationsAsync();
			return View("Create");
		}

		public async Task<IActionResult> PrivacyAsync()
		{
			await FillSubscriptionsAsync();
			await FillNotificationsAsync();
			return View("Privacy");
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error(string message)
		{
			return View(new ErrorViewModel 
			{ 
				RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
				Message = message
			});
		}

		[HttpPost]
		public async Task<IActionResult> CreateAsync(Post post, List<IFormFile> images)
		{
			try
			{
				var author = User.FindFirst(ClaimTypes.Name)?.Value;
				await _postManager.CreateAsync(author, post, images);
				_subscribeManager.Notify(author, $"{author} just posted \"{post.Title}\"");

				return Json(new
				{
					success = true
				});
			}
			catch (Exception ex)
			{
				var message = "Failed to create the post";
				_logger.LogError(ex, message);
				return Json(new
				{
					success = false,
					message
				});
			}
		}

		[HttpPost]
		public async Task<IActionResult> DeleteAsync(string postId)
		{
			try
			{
				await _postManager.DeleteAsync(postId);

				return Json(new
				{
					success = true
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
		public async Task<IActionResult> AddCommentAsync(string postId, string commentContent)
		{
			try
			{
				var author = User.FindFirst(ClaimTypes.Name)?.Value;
				var newComment = await _postManager.AddCommentAsync(author, postId, commentContent);
				_subscribeManager.Notify(author, $"{author} just commented \"{commentContent}\"");

				if (newComment is null)
				{
					return NotFound("Post not found");
				}

				return Json(new
				{
					success = true,
					comment = new
					{
						id = newComment.Id.ToString(),
						author = newComment.Author,
						content = newComment.Content,
						createdAt = newComment.CreatedAt
					}
				});
			}
			catch (Exception ex)
			{
				var message = "Failed to add comment";
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