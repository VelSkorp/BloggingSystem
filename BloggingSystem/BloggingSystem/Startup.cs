using BloggingSystemRepository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;

namespace BloggingSystem
{
	public class Startup
	{
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddRazorPages();
			services.AddControllersWithViews();
			services.AddHttpContextAccessor();

			// Register database services
			services.AddMongoDatabase(Configuration);
			services.AddElasticsearch(Configuration);
			services.AddCeph(Configuration);
			services.AddRedis(Configuration);

			services.AddSingleton<IPostsRepository, PostsRepository>();
			services.AddSingleton<IUserRepository, UserRepository>();
			services.AddSingleton<ISearchService, SearchService>();
			services.AddSingleton<IImageRepository, ImageRepository>(serviceProvider =>
			{
				var cephSettings = serviceProvider.GetRequiredService<IOptions<CephSettings>>().Value;
				var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
				var useHttps = httpContextAccessor.HttpContext.Request.IsHttps;
				var environment = Configuration["ASPNETCORE_ENVIRONMENT"];
				return new ImageRepository(serviceProvider.GetRequiredService<IOptions<CephSettings>>(), environment, useHttps);
			});

			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie(options =>
				{
					options.ExpireTimeSpan = TimeSpan.FromHours(10);
					options.LoginPath = "/Auth/Login";
					options.AccessDeniedPath = "/Auth/AccessDenied";
				});
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Posts/Error");
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapRazorPages();
				endpoints.MapControllers();

				// Set default route to Login page
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Auth}/{action=Login}/{id?}");
			});
		}
	}
}