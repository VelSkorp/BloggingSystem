using BloggingSystemRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Diagnostics;
using System.Security.Claims;

namespace BloggingSystem
{
	[Authorize]
	public class PostsController : Controller
	{
		private readonly ILogger<PostsController> _logger;
		private readonly IPostsRepository _postsRepository;
		private readonly IImageRepository _imageRepository;
		private readonly ISearchService _searchService;

		public PostsController(ILogger<PostsController> logger, IPostsRepository postsRepository, IImageRepository imageRepository, ISearchService searchService)
		{
			_logger = logger;
			_postsRepository = postsRepository;
			_imageRepository = imageRepository;
			_searchService = searchService;
		}

		public async Task<IActionResult> IndexAsync(string author)
		{
			var posts = string.IsNullOrEmpty(author)
				? await _postsRepository.GetAllPostsAsync()
				: await _searchService.SearchPostsByAuthorAsync(author);

			return View("Index", posts.FillPostsWithImageLinks(_imageRepository));
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
				post.CreatedAt = DateTime.Now;
				post.Id = ObjectId.GenerateNewId(post.CreatedAt);
				post.Author = User.FindFirst(ClaimTypes.Name)?.Value;

				if (images is not null && images.Count > 0)
				{
					foreach (var image in images)
					{
						var imageUrl = await _imageRepository.UploadImageAsync(image);
						post.Images.Add(imageUrl);
					}
				}

				await _postsRepository.CreateAsync(post);
				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to create the post");
				return RedirectToAction("Error", "Posts");
			}
		}

		[HttpPost]
		public async Task<IActionResult> AddCommentAsync(string postId, string commentContent)
		{
			try
			{
				var post = await _postsRepository.GetPostByIdAsync(ObjectId.Parse(postId));
				if (post is null)
				{
					return NotFound();
				}

				var newComment = new Comment
				{
					Id = ObjectId.GenerateNewId(DateTime.Now),
					Author = User.FindFirst(ClaimTypes.Name)?.Value,
					Content = commentContent,
					CreatedAt = DateTime.Now,
				};

				post.Comments.Add(newComment);

				await _postsRepository.UpdateAsync(post);

				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to add comment");
				return RedirectToAction("Error", "Posts");
			}
		}
	}
}