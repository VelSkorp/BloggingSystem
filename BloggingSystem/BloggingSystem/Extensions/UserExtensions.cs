using BloggingSystemRepository;

namespace BloggingSystem
{
	public static class UserExtensions
	{
		public static string GetUserPhotoUrl(this User user, IImageRepository imageRepository)
		{
			return user.Photo is null ? "/images/profile-icon.png" : imageRepository.GetImageUrl(user.Photo);
		}
	}
}