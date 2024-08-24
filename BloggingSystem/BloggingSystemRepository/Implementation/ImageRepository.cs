using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace BloggingSystemRepository
{
	public class ImageRepository : IImageRepository
	{
		private readonly IAmazonS3 _s3Client;
		private readonly ITransferUtility _transferUtility;
		private readonly CephSettings _cephSettings;
		private readonly bool _useHttps;
		private readonly string _environment;

		public ImageRepository(IOptions<CephSettings> cephSettings, string environment, bool useHttps)
		{
			_cephSettings = cephSettings.Value;
			
			_s3Client = new AmazonS3Client(_cephSettings.AccessKey, _cephSettings.SecretKey, new AmazonS3Config()
			{
				HttpClientFactory = new BasicHttpClientFactory(),
				ServiceURL = $"{(useHttps ? "https" : "http")}://{_cephSettings.Endpoint}:{(useHttps ? _cephSettings.EndpointHttpsPort : _cephSettings.EndpointPort)}",
				ForcePathStyle = true
			});
			_transferUtility = new TransferUtility(_s3Client);
			_useHttps = useHttps;
			_environment = environment;
		}

		public async Task<string> UploadImageAsync(IFormFile image)
		{
			var responseBucketsList = await _s3Client.ListBucketsAsync();

			if (!responseBucketsList.Buckets.Exists(item => item.BucketName.Equals(_cephSettings.BucketName)))
			{
				await _s3Client.PutBucketAsync(_cephSettings.BucketName);
			}

			var key = Guid.NewGuid().ToString();
			using (var stream = image.OpenReadStream())
			{
				var request = new TransferUtilityUploadRequest
				{
					InputStream = stream,
					Key = key,
					BucketName = _cephSettings.BucketName,
					CannedACL = S3CannedACL.PublicRead
				};

				await _transferUtility.UploadAsync(request);
			}

			return key;
		}

		public string GetImageUrl(string key)
		{
			var port = _environment.Equals("Kubernetes")
				? _useHttps ? _cephSettings.EndpointHttpsExternalPort: _cephSettings.EndpointExternalPort
				: _useHttps ? _cephSettings.EndpointHttpsPort : _cephSettings.EndpointPort;

			return $"{(_useHttps ? "https" : "http")}://localhost:{port}/{_cephSettings.BucketName}/{key}";
		}
	}
}