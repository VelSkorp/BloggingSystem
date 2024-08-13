using BloggingSystemRepository;

namespace BloggingSystem
{
	public static class CephExtensions
	{
		public static IServiceCollection AddCeph(this IServiceCollection services, IConfiguration configuration)
		{
			// Ceph S3 Configuration
			services.Configure<CephSettings>(configuration.GetSection("Ceph"));

			return services;
		}
	}
}