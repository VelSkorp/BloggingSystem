using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BloggingSystemRepository
{
	public class PostsRepository : IPostsRepository
	{
		private readonly IMongoCollection<Post> _postsCollection;

		public PostsRepository(IMongoDatabase mongoDatabase, IOptions<BlogStoreDatabaseSettings> settings)
		{
			_postsCollection = mongoDatabase.GetCollection<Post>(settings.Value.PostsCollectionName);
		}

		public async Task<List<Post>> GetPostsAsync()
		{ 
			return await _postsCollection.Find(_ => true).ToListAsync(); 
		}

		public async Task<Post?> GetPostByIdAsync(int id)
		{
			return await _postsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
		}

		public async Task<List<Post>> GetPostsByAuthorAsync(string author)
		{
			return await _postsCollection.Find(x => x.Author == author).ToListAsync();
		}

		public async Task<Post?> GetPostByAuthorAsync(string author)
		{
			return await _postsCollection.Find(x => x.Author == author).FirstOrDefaultAsync();
		}

		public async Task CreateAsync(Post newPost)
		{
			await _postsCollection.InsertOneAsync(newPost);
		}

		public async Task UpdateAsync(int id, Post updatedPost)
		{
			await _postsCollection.ReplaceOneAsync(x => x.Id == id, updatedPost);
		}

		public async Task RemoveAsync(int id)
		{
			await _postsCollection.DeleteOneAsync(x => x.Id == id);
		}
	}
}