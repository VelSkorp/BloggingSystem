using BloggingSystemRepository;
using Microsoft.AspNetCore.Authentication.Cookies;

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

			services.AddMongoDatabase(Configuration);

			// Register services
			services.AddSingleton<IPostsRepository, PostsRepository>();
			services.AddSingleton<IUserRepository, UserRepository>();

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