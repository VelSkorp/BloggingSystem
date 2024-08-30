using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BloggingSystemRepository
{
	public class PostsRepository : IPostsRepository
	{
		private readonly IMongoCollection<Post> _postsCollection;
		private readonly ElasticsearchClient _elasticClient;

		public PostsRepository(IMongoDatabase mongoDatabase, IOptions<BlogStoreDatabaseSettings> settings, ElasticsearchClient elasticClient)
		{
			_postsCollection = mongoDatabase.GetCollection<Post>(settings.Value.PostsCollectionName);
			_elasticClient = elasticClient;
		}

		public async Task<List<Post>> GetAllPostsAsync()
		{ 
			return await _postsCollection.Find(_ => true).ToListAsync();
		}

		public async Task<Post?> GetPostByIdAsync(ObjectId id)
		{
			return await _postsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
		}

		public async Task CreateAsync(Post newPost)
		{
			await _postsCollection.InsertOneAsync(newPost);
			await _elasticClient.IndexAsync(newPost);
		}

		public async Task UpdateAsync(Post updatedPost)
		{
			await _postsCollection.ReplaceOneAsync(x => x.Id == updatedPost.Id, updatedPost);
			await _elasticClient.UpdateAsync(new UpdateRequest<Post, Post>(index: "posts", id: new Id(updatedPost.Id.ToString()))
			{
				Doc = updatedPost
			});
		}

		public async Task RemoveAsync(ObjectId id)
		{
			var post = await GetPostByIdAsync(id);
			await _postsCollection.DeleteOneAsync(x => x.Id == id);
			await _elasticClient.DeleteAsync(post);
		}
	}
}