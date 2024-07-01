using BloggingSystemRepository;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BloggingSystem
{
	public class PostsController : Controller
	{
		private readonly ILogger<PostsController> _logger;
		private readonly IPostsRepository _postsRepository;

		public PostsController(ILogger<PostsController> logger, IPostsRepository postsRepository)
		{
			_logger = logger;
			_postsRepository = postsRepository;   
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