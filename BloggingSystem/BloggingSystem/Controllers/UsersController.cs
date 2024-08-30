using BloggingSystemRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
		public async Task<IActionResult> UpdateUserAsync(string firstName, string lastName, IFormFile photo)
		{
			try
			{
				string photoUrl = null;

				if (photo is not null)
				{
					photoUrl = await _imageRepository.UploadImageAsync(photo);
					await _userRepository.UpdateUserDetailsAsync(u => u.Photo, photoUrl, User.FindFirst(ClaimTypes.Name)?.Value);
					photoUrl = _imageRepository.GetImageUrl(photoUrl);
				}
				if (firstName is not null)
				{
					await _userRepository.UpdateUserDetailsAsync(u => u.FirstName, firstName, User.FindFirst(ClaimTypes.Name)?.Value);
				}
				if (lastName is not null)
				{
					await _userRepository.UpdateUserDetailsAsync(u => u.LastName, lastName, User.FindFirst(ClaimTypes.Name)?.Value);
				}

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