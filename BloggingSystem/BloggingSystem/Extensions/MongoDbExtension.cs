using BloggingSystemRepository;
using MongoDB.Driver;

namespace BloggingSystem
{
	public static class MongoDbExtension
	{
		public static IServiceCollection AddMongoDatabase(this IServiceCollection services, IConfiguration configuration)
		{
			var mongoClient = new MongoClient(configuration.GetConnectionString("MongoDb"));
			var mongoDatabase = mongoClient.GetDatabase(configuration["BlogStoreDatabase:DatabaseName"]);
			services.AddSingleton(provider => mongoDatabase);
			services.Configure<BlogStoreDatabaseSettings>(configuration.GetSection("BlogStoreDatabase"));
			return services;
		}
	}
}
