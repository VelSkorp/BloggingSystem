using BloggingSystemRepository;

namespace BloggingSystem
{
	public sealed class UserManager
	{
		private readonly IUserRepository _userRepository;
		private readonly IImageRepository _imageRepository;
		private readonly ISearchService _searchService;

		public UserManager(IUserRepository userRepository, IImageRepository imageRepository, ISearchService searchService)
		{
			_userRepository = userRepository;
			_imageRepository = imageRepository;
			_searchService = searchService;
		}

		public async Task<(string username, string photoUrl)> AuthenticateUserAsync(LoginCredentials credentials)
		{
			var user = await _userRepository.AuthenticateUserAsync(credentials);
			return (user.Username, user.Photo is null ? "~/images/profile-icon.png" : _imageRepository.GetImageUrl(user.Photo));
		}

		public async Task<(string username, string photoUrl)> RegisterUserAsync(RegisterCredentials credentials)
		{
			var user = await _userRepository.RegisterUserAsync(credentials);
			return (user.Username, user.Photo is null ? "~/images/profile-icon.png" : _imageRepository.GetImageUrl(user.Photo));

		}

		public async Task<string> UpdateUserDetailsAsync(string username, string firstName, string lastName, IFormFile photo)
		{
			string photoUrl = null;

			if (photo is not null)
			{
				photoUrl = await _imageRepository.UploadImageAsync(photo);
				await _userRepository.UpdateUserDetailsAsync(u => u.Photo, photoUrl, username);
				photoUrl = _imageRepository.GetImageUrl(photoUrl);
			}
			if (firstName is not null)
			{
				await _userRepository.UpdateUserDetailsAsync(u => u.FirstName, firstName, username);
			}
			if (lastName is not null)
			{
				await _userRepository.UpdateUserDetailsAsync(u => u.LastName, lastName, username);
			}

			return photoUrl;
		}

		public async Task<UserDetailsViewModel> GetUserDetailsAsync(string author)
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