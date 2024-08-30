using MongoDB.Bson;

namespace BloggingSystemRepository
{
	public interface IPostsRepository
	{
		Task<List<Post>> GetAllPostsAsync();
		Task<Post?> GetPostByIdAsync(ObjectId id);
		Task CreateAsync(Post newPost);
		Task UpdateAsync(Post updatedPost);
		Task RemoveAsync(ObjectId id);
	}
}