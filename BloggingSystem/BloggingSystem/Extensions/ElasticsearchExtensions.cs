using BloggingSystemRepository;
using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Options;

namespace BloggingSystem
{
	public static class ElasticsearchExtensions
	{
		public static IServiceCollection AddElasticsearch(this IServiceCollection services, IConfiguration configuration)
		{
			// Elasticsearch Configuration
			services.Configure<ElasticsearchSettings>(configuration.GetSection("Elasticsearch"));

			services.AddSingleton(sp =>
			{
				var settings = sp.GetRequiredService<IOptions<ElasticsearchSettings>>().Value;
				var connectionSettings = new ElasticsearchClientSettings(new Uri(configuration.GetConnectionString("Elasticsearch")))
											.DefaultIndex(settings.Index);
				return new ElasticsearchClient(connectionSettings);
			});

			return services;
		}
	}
}