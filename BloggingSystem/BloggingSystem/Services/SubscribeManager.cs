using BloggingSystemRepository;

namespace BloggingSystem
{
	public sealed class SubscribeManager
	{
		private readonly IUserRepository _userRepository;
		private readonly ILogger<SubscribeManager> _logger;

		public SubscribeManager(ILogger<SubscribeManager> logger, IUserRepository userRepository)
		{
			_logger = logger;
			_userRepository = userRepository;
		}

		public async Task SubscribeAsync(string author, string subriber)
		{
			var authorInfo = await _userRepository.GetUserDetailsAsync(author);
			var subriberInfo = await _userRepository.GetUserDetailsAsync(subriber);

			await _userRepository.AddToUserCollectionAsync(u => u.Followers, new UserFollowInfo
			{
				Username = subriberInfo.Username,
				Photo = subriberInfo.Photo
			}, author);

			await _userRepository.AddToUserCollectionAsync(u => u.Following, new UserFollowInfo
			{
				Username = authorInfo.Username,
				Photo = authorInfo.Photo
			}, subriber);
		}

		public async Task UnsubscribeAsync(string author, string subriber)
		{
			var authorInfo = await _userRepository.GetUserDetailsAsync(author);
			var subriberInfo = await _userRepository.GetUserDetailsAsync(subriber);

			await _userRepository.RemoveFromUserCollectionAsync(u => u.Followers, new UserFollowInfo
			{
				Username = subriberInfo.Username,
				Photo = subriberInfo.Photo
			}, author);

			await _userRepository.RemoveFromUserCollectionAsync(u => u.Following, new UserFollowInfo
			{
				Username = authorInfo.Username,
				Photo = authorInfo.Photo
			}, subriber);
		}

		public void Notify(string author, string notification)
		{
			Task.Run(async () =>
			{
				try
				{
					var authorInfo = await _userRepository.GetUserDetailsAsync(author);

					foreach (var follower in authorInfo.Followers)
					{
						await _userRepository.AddToUserCollectionAsync(u => u.Notifications, notification, follower.Username);
					}
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, $"Failed to notify followers of {author}.", author);
				}
			});
		}

		public async Task RemoveNotificationAsync(string subriber, string notification)
		{
			await _userRepository.RemoveFromUserCollectionAsync(u => u.Notifications, notification, subriber);
		}
	}
}