using BloggingSystemRepository;
using MongoDB.Bson;

namespace BloggingSystem
{
	public sealed class PostManager
	{
		private readonly IPostsRepository _postsRepository;
		private readonly IImageRepository _imageRepository;
		private readonly ISearchService _searchService;

		public PostManager(IPostsRepository postsRepository, IImageRepository imageRepository, ISearchService searchService)
		{
			_postsRepository = postsRepository;
			_imageRepository = imageRepository;
			_searchService = searchService;
		}

		public async Task<IEnumerable<Post>> GetPostsAsync(string author)
		{
			var posts = string.IsNullOrEmpty(author)
				? await _postsRepository.GetAllPostsAsync()
				: await _searchService.SearchPostsByAuthorAsync(author);
			return posts.FillPostsWithImageLinkAndSort(_imageRepository);
		}

		public async Task CreateAsync(string author, Post post, List<IFormFile> images)
		{
			post.CreatedAt = DateTime.Now;
			post.Id = ObjectId.GenerateNewId(post.CreatedAt);
			post.Author = author;

			if (images is not null && images.Count > 0)
			{
				foreach (var image in images)
				{
					var imageUrl = await _imageRepository.UploadImageAsync(image);
					post.Images.Add(imageUrl);
				}
			}

			await _postsRepository.CreateAsync(post);
		}

		public async Task DeleteAsync(string postId)
		{
			await _postsRepository.RemoveAsync(ObjectId.Parse(postId));
		}

		public async Task<Comment> AddCommentAsync(string author, string postId, string commentContent)
		{
			var post = await _postsRepository.GetPostByIdAsync(ObjectId.Parse(postId));
			if (post is null)
			{
				return null;
			}

			var newComment = new Comment
			{
				Id = ObjectId.GenerateNewId(DateTime.Now),
				Author = author,
				Content = commentContent,
				CreatedAt = DateTime.Now,
			};

			post.Comments.Add(newComment);

			await _postsRepository.UpdateAsync(post);
			return newComment;
		}
	}
}