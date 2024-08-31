using BloggingSystemRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BloggingSystem
{
	[Authorize]
	public class PostsController : Controller
	{
		private readonly ILogger<PostsController> _logger;
		private readonly PostManager _postManager;
		private readonly SubscribeManager _subscribeManager;

		public PostsController(ILogger<PostsController> logger, SubscribeManager subscribeManager, PostManager postManager)
		{
			_logger = logger;
			_postManager = postManager;
			_subscribeManager = subscribeManager;
		}

		public async Task<IActionResult> IndexAsync(string author)
		{
			return View("Index", await _postManager.GetPostsAsync(author));
		}

		public IActionResult Create()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		[HttpPost]
		public async Task<IActionResult> CreateAsync(Post post, List<IFormFile> images)
		{
			try
			{
				var author = User.FindFirst(ClaimTypes.Name)?.Value;
				await _postManager.CreateAsync(author, post, images);
				_subscribeManager.Notify(author, $"{author} just posted \"{post.Title}\"");
				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to create the post");
				return RedirectToAction("Error", "Posts");
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
				_logger.LogError(ex, "Failed to delete the post");
				return RedirectToAction("Error", "Posts");
			}
		}

		[HttpPost]
		public async Task<IActionResult> AddCommentAsync(string postId, string commentContent)
		{
			try
			{
				var author = User.FindFirst(ClaimTypes.Name)?.Value;
				var newComment = await _postManager.AddCommentAsync(author, postId, commentContent);

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
				_logger.LogError(ex, "Failed to add comment");
				return RedirectToAction("Error", "Posts");
			}
		}
	}
}