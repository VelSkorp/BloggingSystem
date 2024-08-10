using BloggingSystemRepository;
using MongoDB.Driver;

namespace BloggingSystem
{
	public static class MongoDbExtensions
	{
		public static IServiceCollection AddMongoDatabase(this IServiceCollection services, IConfiguration configuration)
		{
			// MongoDb Configuration
			var mongoClient = new MongoClient(configuration.GetConnectionString("MongoDb"));

			services.AddSingleton(provider => mongoClient.GetDatabase(configuration["BlogStoreDatabase:DatabaseName"]));
			services.Configure<BlogStoreDatabaseSettings>(configuration.GetSection("BlogStoreDatabase"));

			return services;
		}
	}
}