using BloggingSystemRepository;

namespace BloggingSystem
{
	public static class SubscriptionsExtensions
	{
		public static HashSet<UserFollowInfo> FillSubscriptionsWithImageLinks(this HashSet<UserFollowInfo> subscriptions, IImageRepository imageRepository)
		{
			foreach (var subscription in subscriptions)
			{
				subscription.Photo = subscription.Photo is null ? "/images/profile-icon.png" : imageRepository.GetImageUrl(subscription.Photo);
			}

			return subscriptions;
		}
	}
}