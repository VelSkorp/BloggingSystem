using BloggingSystemRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace BloggingSystem
{
	[Authorize]
	public class UsersController : Controller
	{
		private readonly ILogger<PostsController> _logger;
		private readonly IUserRepository _userRepository;
		private readonly IPostsRepository _postsRepository;
		private readonly IImageRepository _imageRepository;
		private readonly ISearchService _searchService;

		public UsersController(ILogger<PostsController> logger, IUserRepository userRepository, IPostsRepository postsRepository, IImageRepository imageRepository, ISearchService searchService)
		{
			_logger = logger;
			_userRepository = userRepository;
			_postsRepository = postsRepository;
			_imageRepository = imageRepository;
			_searchService = searchService;
		}

		public async Task<IActionResult> AuthorDetailsAsync(string author)
		{
			if (author is null)
			{
				return NotFound();
			}

			return View("AuthorDetails", await GetUserDetailsAsync(author));
		}

		public async Task<IActionResult> ProfileDetailsAsync(string author)
		{
			if (author is null)
			{
				return NotFound();
			}

			return View("ProfileDetails", await GetUserDetailsAsync(author));
		}

		[HttpPost]
		public async Task<IActionResult> UpdateUserAsync(User user)
		{
			if (user is null)
			{
				return Json(new { success = false, message = "User data is null" });
			}

			try
			{
				await _userRepository.UpdateUserDetailsAsync(user);

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

		private async Task<UserDetailsViewModel> GetUserDetailsAsync(string author)
		{

			var posts = await _searchService.SearchPostsByAuthorAsync(author);
			var user = await _userRepository.GetUserDetailsAsync(author);

			if (user.Photo is not null)
			{
				user.Photo = _imageRepository.GetImageUrl(user.Photo);
			}

			return new UserDetailsViewModel()
			{
				Posts = posts.FillPostsWithImageLinkAndSort(_imageRepository),
				User = user
			};
		}
	}
}