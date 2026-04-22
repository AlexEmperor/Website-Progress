using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Website_Progress.Helpers;

namespace Website_Progress.Areas.Admin.Controllers
{
    [Area(Constants.AdminRoleName)]
    [Authorize(Roles = Constants.AdminRoleName)]
    public class MigrationController : Controller
    {
        private readonly IProductRepository _products;
        private readonly INewsRepository _news;
        private readonly IFileStorage _storage;
        private readonly IWebHostEnvironment _env;

        public MigrationController(
            IProductRepository products,
            INewsRepository news,
            IFileStorage storage,
            IWebHostEnvironment env)
        {
            _products = products;
            _news = news;
            _storage = storage;
            _env = env;
        }

        public async Task<IActionResult> Run()
        {
            var log = new List<string>
            {
                "=== MIGRATION START ===",
                $"WebRootPath: {_env.WebRootPath}",
                "",

                // ======= PRODUCTS =======
                "--- PRODUCTS ---"
            };
            var products = await _products.GetAllAsync();

            foreach (var p in products)
            {
                log.Add($"Product #{p.Id}: {p.Name}");

                // Фото (может быть несколько через ';')
                var photos = (p.PhotoPath ?? "")
                    .Split(';', StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim())
                    .Where(x => !string.IsNullOrEmpty(x))
                    .ToList();

                var newPhotos = new List<string>();
                foreach (var oldPath in photos)
                {
                    var migrated = await MigrateOne(oldPath, "products", p.Name, log);
                    newPhotos.Add(migrated ?? oldPath); // не смогли — оставим как было
                }
                if (newPhotos.Any())
                {
                    p.PhotoPath = string.Join(";", newPhotos);
                }

                // Презентация
                var pres = await MigrateOne(p.PresentationPath, "presentations", p.Name, log);
                if (pres != null)
                {
                    p.PresentationPath = pres;
                }

                // Прошивка
                var fw = await MigrateOne(p.FirmwarePath, "firmware", p.Name, log);
                if (fw != null)
                {
                    p.FirmwarePath = fw;
                }

                await _products.UpdateAsync(p);
                log.Add("");
            }

            // ======= NEWS =======
            log.Add("--- NEWS ---");
            var allNews = await _news.GetAllAsync();
            foreach (var n in allNews)
            {
                log.Add($"News #{n.Id}: {n.Title}");
                var migrated = await MigrateOne(n.ImageUrl, "news", n.Title, log);
                if (migrated != null)
                {
                    n.ImageUrl = migrated;
                    await _news.UpdateAsync(n);
                }
                log.Add("");
            }

            log.Add("=== MIGRATION DONE ===");
            return Content(string.Join("\n", log), "text/plain; charset=utf-8");
        }

        private async Task<string?> MigrateOne(string? oldPath, string folder, string baseName, List<string> log)
        {
            if (string.IsNullOrWhiteSpace(oldPath))
            {
                return null;
            }

            // Уже мигрировано (абсолютный URL) — пропускаем
            if (oldPath.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                oldPath.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                log.Add($"    · SKIP (already migrated): {oldPath}");
                return null;
            }

            var physical = Path.Combine(
                _env.WebRootPath,
                oldPath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

            if (!System.IO.File.Exists(physical))
            {
                log.Add($"    ✗ FILE NOT FOUND: {physical}");
                return null;
            }

            try
            {
                await using var fs = System.IO.File.OpenRead(physical);
                var formFile = new FormFile(fs, 0, fs.Length, "file", Path.GetFileName(physical))
                {
                    Headers = new HeaderDictionary(),
                    ContentType = GetContentType(physical)
                };

                var url = await _storage.SaveAsync(formFile, folder, baseName);
                log.Add($"    ✓ {oldPath} → {url}");
                return url;
            }
            catch (Exception ex)
            {
                log.Add($"    ✗ ERROR ({oldPath}): {ex.Message}");
                return null;
            }
        }

        private static string GetContentType(string path) =>
            Path.GetExtension(path).ToLowerInvariant() switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".webp" => "image/webp",
                ".svg" => "image/svg+xml",
                ".pdf" => "application/pdf",
                ".pptx" => "application/vnd.openxmlformats-officedocument.presentationml.presentation",
                ".ppt" => "application/vnd.ms-powerpoint",
                ".zip" => "application/zip",
                ".bin" or ".hex" => "application/octet-stream",
                _ => "application/octet-stream"
            };
    }
}