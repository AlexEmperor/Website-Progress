namespace Website_Progress.Helpers
{
    public class YandexS3Settings
    {
        public string AccessKey { get; set; } = "";
        public string SecretKey { get; set; } = "";
        public string Bucket { get; set; } = "";
        public string Endpoint { get; set; } = "https://storage.yandexcloud.net";
    }

    public class YandexS3FileStorage : IFileStorage
    {
        private readonly YandexS3Settings _settings;
        private readonly AmazonS3Client _s3;

        public YandexS3FileStorage(IOptions<YandexS3Settings> options)
        {
            _settings = options.Value;

            var config = new AmazonS3Config
            {
                ServiceURL = _settings.Endpoint,
                ForcePathStyle = true,
                AuthenticationRegion = "ru-central1"
            };

            _s3 = new AmazonS3Client(_settings.AccessKey, _settings.SecretKey, config);
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

            using var stream = file.OpenReadStream();

            var request = new PutObjectRequest
            {
                BucketName = _settings.Bucket,
                Key = objectKey,
                InputStream = stream,
                ContentType = file.ContentType,
                DisablePayloadSigning = true
            };

            await _s3.PutObjectAsync(request);

            return $"{_settings.Endpoint.TrimEnd('/')}/{_settings.Bucket}/{objectKey}";
        }

        public async Task DeleteAsync(string? publicUrl)
        {
            if (string.IsNullOrWhiteSpace(publicUrl))
            {
                return;
            }

            var prefix = $"{_settings.Endpoint.TrimEnd('/')}/{_settings.Bucket}/";
            if (!publicUrl.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            var key = publicUrl.Substring(prefix.Length);

            try
            {
                await _s3.DeleteObjectAsync(new DeleteObjectRequest
                {
                    BucketName = _settings.Bucket,
                    Key = key
                });
            }
            catch (AmazonS3Exception)
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