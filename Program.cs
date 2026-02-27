using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Website_Progress;
using Website_Progress.DataContext;
using Website_Progress.Interfaces;
using Website_Progress.ModelsDTO;
using Website_Progress.Repositories;
using Website_Progress.Services;

var builder = WebApplication.CreateBuilder(args);
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
string connection = builder.Configuration.GetConnectionString("WebTestConnection");

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IProductRepository, ProductsDbRepository>();
builder.Services.AddTransient<INewProductRepository, InMemoryNewProductRepository>();
builder.Services.AddTransient<INewsRepository, NewsDbRepository>();
builder.Services.AddTransient<ICartRepository, CartDbRepository>();
builder.Services.AddTransient<IOrderRepository, OrdersDbRepository>();
//builder.Services.AddSingleton<IRoleRepository, InMemoryRoleRepository>();
//builder.Services.AddSingleton<IUserRepository, InMemoryUserRepository>();

var botToken = builder.Configuration["Telegram:BotToken"];

builder.Services.AddSingleton(new TelegramBotClient(botToken));

builder.Services.AddScoped<TelegramService>();


builder.Services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(connection));

builder.Services.AddDbContext<IdentityContext>(options => options.UseNpgsql(connection));
builder.Services.AddIdentity<UserDTO, IdentityRole>().AddEntityFrameworkStores<IdentityContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromHours(1);
    options.LoginPath = "/Account/Autorization";
    options.LogoutPath = "/Account/Logout";
    options.Cookie = new CookieBuilder
    {
        IsEssential = true
    };
});


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

app.MapControllerRoute(
    name: "MyArea",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserDTO>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    IdentityInitializer.Inititalize(userManager, roleManager);
}

app.Run();
