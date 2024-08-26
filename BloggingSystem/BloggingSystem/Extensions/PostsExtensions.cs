using BloggingSystemRepository;

namespace BloggingSystem
{
	public static class PostsExtensions
	{
		public static IEnumerable<Post> FillPostsWithImageLinks(this List<Post> posts, IImageRepository imageRepository)
		{
			posts.ForEach(post =>
			{
				if (post.Images.All(image => image is not null))
				{
					for (var i = 0; i < post.Images.Count; i++)
					{
						post.Images[i] = imageRepository.GetImageUrl(post.Images[i]);
					}
				}
			});

			return posts.Reverse<Post>();

		}
	}
}