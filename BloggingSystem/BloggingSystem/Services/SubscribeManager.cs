using BloggingSystemRepository;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace BloggingSystem
{
    public sealed class SubscribeManager
    {
        private readonly IUserRepository _userRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IDistributedCache _cache;
        private readonly ILogger<SubscribeManager> _logger;

        public SubscribeManager(ILogger<SubscribeManager> logger, IUserRepository userRepository, IImageRepository imageRepository, IDistributedCache cache)
        {
            _logger = logger;
            _userRepository = userRepository;
            _imageRepository = imageRepository;
            _cache = cache;
        }

        public async Task<HashSet<UserFollowInfo>> GetSubscriptionsAsync(string username)
        {
            var cacheKey = $"subscriptions_{username}";
            var cachedData = await _cache.GetStringAsync(cacheKey);

            if (cachedData is not null)
            {
                return JsonConvert.DeserializeObject<HashSet<UserFollowInfo>>(cachedData);
            }

            var user = await _userRepository.GetUserDetailsAsync(username);

            await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(user.Following.FillSubscriptionsWithImageLinks(_imageRepository)),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                });

            return user.Following;
        }

        public async Task<UserFollowInfo> SubscribeAsync(string author, string subriber)
        {
            var authorInfo = await _userRepository.GetUserDetailsAsync(author);
            var subriberInfo = await _userRepository.GetUserDetailsAsync(subriber);

            await _userRepository.AddToUserCollectionAsync(u => u.Followers, new UserFollowInfo
            {
                Username = subriberInfo.Username,
                Photo = subriberInfo.Photo
            }, author);

            var subscription = new UserFollowInfo
            {
                Username = authorInfo.Username,
                Photo = authorInfo.Photo
            };

            await _userRepository.AddToUserCollectionAsync(u => u.Following, subscription, subriber);

            subscription.Photo = authorInfo.GetUserPhotoUrl(_imageRepository);

            return subscription;
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