namespace Website_Progress
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add services to the container.
            services.AddControllersWithViews();
            services.AddTransient<IProductRepository, ProductsDbRepository>();
            services.AddTransient<INewsRepository, NewsDbRepository>();
            services.AddTransient<ICartRepository, CartDbRepository>();
            services.AddTransient<IOrderRepository, OrdersDbRepository>();

            services.Configure<YandexS3Settings>(configuration.GetSection("YandexS3"));
            services.AddSingleton<IFileStorage, YandexS3FileStorage>();

            var botToken = configuration["Telegram:BotToken"]!;
            services.AddSingleton(new TelegramBotClient(botToken));
            services.AddScoped<TelegramService>();

            var connection = configuration.GetConnectionString("WebTestConnection")!;
            services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(connection));
            services.AddDbContext<IdentityContext>(options => options.UseNpgsql(connection));

            services.AddIdentity<UserDTO, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 1;
                options.Password.RequiredUniqueChars = 0;
            }).AddEntityFrameworkStores<IdentityContext>();

            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssemblyContaining<RegistrationViewModelValidator>();

            services.ConfigureApplicationCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromHours(10);
                options.LoginPath = "/Account/Autorization";
                options.LogoutPath = "/Account/Logout";
                options.Cookie = new CookieBuilder
                {
                    IsEssential = true
                };
            });

            services.AddMemoryCache();
            // Сжатие ответов (HTML/JSON/CSS/JS)
            services.AddResponseCompression(opt =>
            {
                opt.EnableForHttps = true;
                opt.Providers.Add<BrotliCompressionProvider>();
                opt.Providers.Add<GzipCompressionProvider>();
            });

            return services;
        }

        public static WebApplication UseServices(this WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    // Картинки кэшируем на неделю
                    ctx.Context.Response.Headers.Append(
                        "Cache-Control", "public, max-age=604800");
                }
            });

            app.UseResponseCompression();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<SiteModeMiddleware>();

            app.MapControllerRoute(
                name: "MyArea",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var db = services.GetRequiredService<DatabaseContext>();
                db.Database.Migrate();

                var identity = services.GetRequiredService<IdentityContext>();
                identity.Database.Migrate();

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserDTO>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                IdentityInitializer.Inititalize(userManager, roleManager);
            }

            return app;
        }
    }
}
