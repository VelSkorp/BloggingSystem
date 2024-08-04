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

		public PostsController(ILogger<PostsController> logger, IPostsRepository postsRepository, IImageRepository imageRepository)
		{
			_logger = logger;
			_postsRepository = postsRepository;
			_imageRepository = imageRepository;   
		}

		public async Task<IActionResult> IndexAsync(string author)
		{
			var posts = string.IsNullOrEmpty(author)
				? await _postsRepository.GetPostsAsync()
				: await _postsRepository.GetPostsByAuthorAsync(author);

			posts.ForEach(post =>
			{
				if (post.Images.All(image => image != null))
				{
					for (var i = 0; i < post.Images.Count; i++)
					{
						post.Images[i] = _imageRepository.GetImageUrl(post.Images[i]);
					}
				}
			});

			return View("Index", posts.Reverse<Post>());
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