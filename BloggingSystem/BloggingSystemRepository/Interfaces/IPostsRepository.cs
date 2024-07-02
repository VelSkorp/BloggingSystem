using MongoDB.Bson;

namespace BloggingSystemRepository
{
	public interface IPostsRepository
	{
		Task<List<Post>> GetPostsAsync();
		Task<Post?> GetPostByIdAsync(ObjectId id);
		Task<List<Post>> GetPostsByAuthorAsync(string author);
		Task<Post?> GetPostByAuthorAsync(string author);
		Task CreateAsync(Post newPost);
		Task UpdateAsync(Post updatedPost);
		Task RemoveAsync(Post post);
	}
}