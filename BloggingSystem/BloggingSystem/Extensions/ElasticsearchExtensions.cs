using BloggingSystemRepository;
using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Microsoft.Extensions.Options;
using System.Security.Cryptography.X509Certificates;

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
				var certificatePath = configuration.GetSection("Certificate:Path").Value;
				var certificatePassword = configuration.GetSection("Certificate:Password").Value;
				var connectionSettings = new ElasticsearchClientSettings(new Uri(configuration.GetConnectionString("Elasticsearch")))
											.DefaultIndex(settings.Index)
											.Authentication(new BasicAuthentication(settings.User, settings.Password))
											.ClientCertificate(new X509Certificate2(certificatePath, certificatePassword))
											.ServerCertificateValidationCallback((sender, certificate, chain, sslPolicyErrors) => true);
				return new ElasticsearchClient(connectionSettings);
			});

			return services;
		}
	}
}