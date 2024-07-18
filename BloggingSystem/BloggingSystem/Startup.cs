using BloggingSystemRepository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;
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
			services.AddHttpContextAccessor();

			// Ceph S3 Configuration
			services.Configure<CephSettings>(Configuration.GetSection("Ceph"));

			// Register database services
			services.AddMongoDatabase(Configuration);
			services.AddSingleton<IPostsRepository, PostsRepository>();
			services.AddSingleton<IUserRepository, UserRepository>();
			services.AddSingleton<IImageRepository, ImageRepository>(serviceProvider =>
			{
				var cephSettings = serviceProvider.GetRequiredService<IOptions<CephSettings>>().Value;
				var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
				var useHttps = httpContextAccessor.HttpContext.Request.IsHttps;
				return new ImageRepository(serviceProvider.GetRequiredService<IOptions<CephSettings>>(), useHttps);
			});

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
			});
		}
	}
}