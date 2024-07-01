namespace BloggingSystemRepository
{
	public interface IPostsRepository
	{
		Task<List<Post>> GetPostsAsync();
		Task<Post?> GetPostByIdAsync(int id);
		Task<List<Post>> GetPostsByAuthorAsync(string author);
		Task<Post?> GetPostByAuthorAsync(string author);
		Task CreateAsync(Post newPost);
		Task UpdateAsync(int id, Post updatedPost);
		Task RemoveAsync(int id);
	}
}