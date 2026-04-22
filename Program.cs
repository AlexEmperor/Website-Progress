using Website_Progress.Helpers;

var builder = WebApplication.CreateBuilder(args);

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var ruCulture = new CultureInfo("ru-RU");
CultureInfo.DefaultThreadCurrentCulture = ruCulture;
CultureInfo.DefaultThreadCurrentUICulture = ruCulture;

string connection = builder.Configuration.GetConnectionString("WebTestConnection")!;
Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IProductRepository, ProductsDbRepository>();
builder.Services.AddTransient<INewsRepository, NewsDbRepository>();
builder.Services.AddTransient<ICartRepository, CartDbRepository>();
builder.Services.AddTransient<IOrderRepository, OrdersDbRepository>();
builder.Services.Configure<SupabaseSettings>(builder.Configuration.GetSection("Supabase"));
builder.Services.AddSingleton<IFileStorage, SupabaseFileStorage>();

var botToken = builder.Configuration["Telegram:BotToken"]!;

builder.Services.AddSingleton(new TelegramBotClient(botToken));

builder.Services.AddScoped<TelegramService>();


builder.Services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(connection));

builder.Services.AddDbContext<IdentityContext>(options => options.UseNpgsql(connection));
builder.Services.AddIdentity<UserDTO, IdentityRole>().AddEntityFrameworkStores<IdentityContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromHours(10);
    options.LoginPath = "/Account/Autorization";
    options.LogoutPath = "/Account/Logout";
    options.Cookie = new CookieBuilder
    {
        IsEssential = true
    };
});
builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

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

app.Run();
