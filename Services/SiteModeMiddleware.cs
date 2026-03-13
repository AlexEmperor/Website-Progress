using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Website_Progress.DataContext;

namespace Website_Progress.Services
{
    public class SiteModeMiddleware
    {
        private readonly RequestDelegate _next;

        public SiteModeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, DatabaseContext db, IMemoryCache cache)
        {
            var settings = await cache.GetOrCreateAsync("SiteSettings", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60);
                return await db.SiteSettings.FirstOrDefaultAsync();
            });

            if (settings != null)
            {
                var path = context.Request.Path.Value ?? "";

                // --- Разрешаем всегда админку и страницы Account (логин, выход, регистрация) ---
                bool isAdminArea = path.StartsWith("/Admin", StringComparison.OrdinalIgnoreCase);
                bool isAccount = path.StartsWith("/Account", StringComparison.OrdinalIgnoreCase);
                bool isStatic = path.StartsWith("/css") || path.StartsWith("/js") || path.StartsWith("/images");

                // --- Технический перерыв ---
                if (settings.Mode == SiteMode.Maintenance)
                {
                    if (!context.User.IsInRole("Admin") && !isAdminArea && !isAccount && !isStatic && !path.StartsWith("/Maintenance"))
                    {
                        context.Response.Redirect("/Maintenance");
                        return;
                    }
                }
            }

            await _next(context);
        }
    }
}
