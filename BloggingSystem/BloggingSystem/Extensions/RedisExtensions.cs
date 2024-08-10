using Microsoft.AspNetCore.DataProtection;
using StackExchange.Redis;

namespace BloggingSystem
{
	public static class RedisExtensions
	{
		public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
		{
			// Configure data protection to use Redis
			services.AddStackExchangeRedisCache(options =>
			{
				options.Configuration = configuration.GetConnectionString("Redis");
			});

			services.AddDataProtection()
				.PersistKeysToStackExchangeRedis(ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis")), "DataProtection-Keys")
				.SetApplicationName("BloggingSystem");

			services.AddAntiforgery(options =>
			{
				options.Cookie.Name = "X-CSRF-TOKEN";
				options.HeaderName = "X-CSRF-TOKEN-HEADER";
			});

			return services;
		}
	}
}