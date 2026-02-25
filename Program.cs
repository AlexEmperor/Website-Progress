using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Website_Progress.DataContext;
using Website_Progress.Interfaces;
using Website_Progress.Repositories;
using Website_Progress.Services;

var builder = WebApplication.CreateBuilder(args);
string connection = builder.Configuration.GetConnectionString("WebTestConnection");

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IProductRepository, ProductsDbRepository>();
builder.Services.AddTransient<INewProductRepository, InMemoryNewProductRepository>();
builder.Services.AddTransient<INewsRepository, NewsDbRepository>();
builder.Services.AddTransient<ICartRepository, CartDbRepository>();
builder.Services.AddTransient<IOrderRepository, OrdersDbRepository>();

var botToken = builder.Configuration["Telegram:BotToken"];

builder.Services.AddSingleton(new TelegramBotClient(botToken));

builder.Services.AddScoped<TelegramService>();


builder.Services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(connection));


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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
