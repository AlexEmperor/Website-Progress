using Microsoft.Extensions.Options;
using Supabase;

namespace Website_Progress.Helpers
{
    public interface IFileStorage
    {
        Task<string?> SaveAsync(IFormFile? file, string folder, string? baseName = null);
        Task DeleteAsync(string? publicUrl);
    }
    public class SupabaseSettings
    {
        public string Url { get; set; } = "";
        public string ServiceKey { get; set; } = "";
        public string Bucket { get; set; } = "files";
    }

    public class SupabaseFileStorage : IFileStorage
    {
        private readonly SupabaseSettings _settings;
        private readonly Client _client;

        public SupabaseFileStorage(IOptions<SupabaseSettings> options)
        {
            _settings = options.Value;

            var sbOptions = new SupabaseOptions
            {
                AutoConnectRealtime = false,
                AutoRefreshToken = false
            };

            _client = new Client(_settings.Url, _settings.ServiceKey, sbOptions);
            _client.InitializeAsync().GetAwaiter().GetResult();
        }

        public async Task<string?> SaveAsync(IFormFile? file, string folder, string? baseName = null)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            var safeBase = Transliterate(string.IsNullOrWhiteSpace(baseName) ? "file" : baseName);
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            var unique = Guid.NewGuid().ToString("N").Substring(0, 8);
            var objectKey = $"{folder}/{safeBase}_{unique}{ext}";

            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            var bytes = ms.ToArray();

            var bucket = _client.Storage.From(_settings.Bucket);

            await bucket.Upload(bytes, objectKey, new Supabase.Storage.FileOptions
            {
                ContentType = file.ContentType,
                Upsert = false
            });

            return bucket.GetPublicUrl(objectKey);
        }

        public async Task DeleteAsync(string? publicUrl)
        {
            if (string.IsNullOrWhiteSpace(publicUrl))
            {
                return;
            }

            var marker = $"/object/public/{_settings.Bucket}/";
            var idx = publicUrl.IndexOf(marker, StringComparison.OrdinalIgnoreCase);
            if (idx < 0)
            {
                return;
            }

            var key = publicUrl.Substring(idx + marker.Length);

            try
            {
                await _client.Storage.From(_settings.Bucket).Remove([key]);
            }
            catch
            {
                // уже удалён — игнорим
            }
        }

        private static string Transliterate(string input)
        {
            var map = new Dictionary<char, string>
            {
                ['а'] = "a",
                ['б'] = "b",
                ['в'] = "v",
                ['г'] = "g",
                ['д'] = "d",
                ['е'] = "e",
                ['ё'] = "yo",
                ['ж'] = "zh",
                ['з'] = "z",
                ['и'] = "i",
                ['й'] = "y",
                ['к'] = "k",
                ['л'] = "l",
                ['м'] = "m",
                ['н'] = "n",
                ['о'] = "o",
                ['п'] = "p",
                ['р'] = "r",
                ['с'] = "s",
                ['т'] = "t",
                ['у'] = "u",
                ['ф'] = "f",
                ['х'] = "h",
                ['ц'] = "ts",
                ['ч'] = "ch",
                ['ш'] = "sh",
                ['щ'] = "sch",
                ['ъ'] = "",
                ['ы'] = "y",
                ['ь'] = "",
                ['э'] = "e",
                ['ю'] = "yu",
                ['я'] = "ya"
            };

            var result = new System.Text.StringBuilder();
            foreach (var ch in input.ToLowerInvariant())
            {
                if (map.TryGetValue(ch, out var t))
                {
                    result.Append(t);
                }
                else if (char.IsLetterOrDigit(ch))
                {
                    result.Append(ch);
                }
                else if (ch == ' ' || ch == '-' || ch == '_')
                {
                    result.Append('-');
                }
            }

            var s = result.ToString().Trim('-');
            return string.IsNullOrEmpty(s) ? "file" : s;
        }
    }
}
