using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BloggingSystem
{
	[ApiController]
	[Route("api/[controller]")]
	public class PostsController : Controller
	{
		private readonly ILogger<PostsController> _logger;
		private readonly PostsService _postsService;

		public PostsController(ILogger<PostsController> logger, PostsService postsService)
		{
			_logger = logger;
			_postsService = postsService;   
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}