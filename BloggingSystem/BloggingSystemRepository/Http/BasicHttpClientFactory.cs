using Amazon.Runtime;
using System.Net.Security;

namespace BloggingSystemRepository
{
	public class BasicHttpClientFactory : HttpClientFactory
	{
		public override HttpClient CreateHttpClient(IClientConfig clientConfig)
		{
			// Create HttpClientHandler with custom certificate validation
			var handler = new HttpClientHandler
			{
				ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
				{
					if (sslPolicyErrors == SslPolicyErrors.RemoteCertificateNameMismatch)
					{
						// Ignore the name mismatch
						return true;
					}
					return sslPolicyErrors == SslPolicyErrors.None;
				}
			};

			// Return an HttpClient with a custom HttpClientHandler
			return new HttpClient(handler);
		}
	}
}