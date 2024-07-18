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

		public ImageRepository(IOptions<CephSettings> cephSettings, bool useHttps)
		{
			_cephSettings = cephSettings.Value;

			_s3Client = new AmazonS3Client(_cephSettings.AccessKey, _cephSettings.SecretKey, new AmazonS3Config()
			{
				// Use this with the production ssl certificate
				//ServiceURL = $"{(useHttps ? "https" : "http")}://{_cephSettings.Endpoint}:{(useHttps ? _cephSettings.EndpointHttpsPort : _cephSettings.EndpointPort)}",
				ServiceURL = $"http://{_cephSettings.Endpoint}:{_cephSettings.EndpointPort}",
				ForcePathStyle = true
			});
			_transferUtility = new TransferUtility(_s3Client);
			_useHttps = useHttps;
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
			return $"{(_useHttps ? "https" : "http")}://localhost:{(_useHttps ? _cephSettings.EndpointHttpsPort : _cephSettings.EndpointPort)}/{_cephSettings.BucketName}/{key}";
		}
	}
}