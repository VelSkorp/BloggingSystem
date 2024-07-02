using BloggingSystemRepository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using StackExchange.Redis;

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

			// Configure data protection to use Redis
			services.AddStackExchangeRedisCache(options =>
			{
				options.Configuration = Configuration.GetConnectionString("RedisConnection");
			});

			services.AddDataProtection()
				.PersistKeysToStackExchangeRedis(ConnectionMultiplexer.Connect(Configuration.GetConnectionString("RedisConnection")), "DataProtection-Keys")
				.SetApplicationName("BloggingSystem");

			services.AddAntiforgery(options =>
			{
				options.Cookie.Name = "X-CSRF-TOKEN";
				options.HeaderName = "X-CSRF-TOKEN-HEADER";
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
				//pattern: "{controller=Posts}/{action=Index}/{id?}");
			});
		}
	}
}