using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Options;
using MongoDB.Bson;

namespace BloggingSystemRepository
{
	public class SearchService : ISearchService
	{
		private readonly ElasticsearchClient _elasticClient;
		private readonly ElasticsearchSettings _elasticsearchSettings;

		public SearchService(ElasticsearchClient elasticClient, IOptions<ElasticsearchSettings> elasticsearchSettings)
		{
			_elasticClient = elasticClient;
			_elasticsearchSettings = elasticsearchSettings.Value;
		}

		public async Task<List<Post>> SearchPostsByAuthorAsync(string author)
		{
			var searchResponse = await _elasticClient.SearchAsync<Post>(search => search
				.Index(_elasticsearchSettings.Index)
				.Query(query => query
					.Term(term => term
						.Field(field => field.Author)
						.Value(author.ToLower())
					)
				)
			);

			return searchResponse.Hits.Select(h =>
			{
				h.Source.Id = ObjectId.Parse(h.Id);
				return h.Source;
			}).ToList();
		}
	}
}