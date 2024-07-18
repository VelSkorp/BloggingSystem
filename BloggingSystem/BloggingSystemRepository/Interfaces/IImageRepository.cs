using Microsoft.AspNetCore.Http;

namespace BloggingSystemRepository
{
	public interface IImageRepository
	{
		Task<string> UploadImageAsync(IFormFile image);
		string GetImageUrl(string key);
	}
}