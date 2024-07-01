using BloggingSystemRepository;

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
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

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
