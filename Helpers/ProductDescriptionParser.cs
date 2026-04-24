namespace Website_Progress.Helpers
{
    public class ProductContent
    {
        public List<FeatureCard> Features { get; set; } = [];
        public List<string> Highlights { get; set; } = [];
        public List<SpecRow> Specs { get; set; } = [];
        public List<UseCase> UseCases { get; set; } = [];
        public string? PlainText { get; set; }
        public bool HasStructured =>
            Features.Any() || Highlights.Any() || Specs.Any() || UseCases.Any();
    }

    public class FeatureCard
    {
        public string Title { get; set; } = "";
        public string Icon { get; set; } = "bi-check2-circle";
        public string Text { get; set; } = "";
    }

    public class SpecRow
    {
        public string Number { get; set; } = "";
        public string Param { get; set; } = "";
        public string Value { get; set; } = "";
    }

    public class UseCase
    {
        public string Title { get; set; } = "";
        public string Text { get; set; } = "";
    }

    public static class ProductDescriptionParser
    {
        public static ProductContent Parse(string? description)
        {
            var content = new ProductContent();
            if (string.IsNullOrWhiteSpace(description))
            {
                return content;
            }

            // Если в описании нет ни одного маркера "## " — это простой текст
            if (!description.Contains("## "))
            {
                content.PlainText = description;
                return content;
            }

            // Режем на разделы по "## "
            var sections = Regex.Split(description, @"(?m)^##\s+")
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .ToList();

            foreach (var raw in sections)
            {
                var lines = raw.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                               .Select(l => l.TrimEnd('\r'))
                               .ToList();
                if (lines.Count == 0)
                {
                    continue;
                }

                var header = lines[0].Trim().ToLowerInvariant();
                var body = lines.Skip(1).ToList();

                if (header.StartsWith("возможност"))
                {
                    content.Features = ParseFeatures(body);
                }
                else if (header.StartsWith("особенност"))
                {
                    content.Highlights = ParseHighlights(body);
                }
                else if (header.StartsWith("характеристик") || header.StartsWith("параметр"))
                {
                    content.Specs = ParseSpecs(body);
                }
                else if (header.StartsWith("применени") || header.StartsWith("сфер"))
                {
                    content.UseCases = ParseUseCases(body);
                }
            }

            return content;
        }

        private static List<FeatureCard> ParseFeatures(List<string> lines)
        {
            var result = new List<FeatureCard>();
            FeatureCard? current = null;

            foreach (var line in lines)
            {
                var t = line.Trim();
                if (t.StartsWith("### "))
                {
                    if (current != null)
                    {
                        result.Add(current);
                    }

                    var headerLine = t.Substring(4).Trim();
                    var parts = headerLine.Split('|', 2);
                    current = new FeatureCard
                    {
                        Title = parts[0].Trim(),
                        Icon = parts.Length > 1 ? parts[1].Trim() : "bi-check2-circle"
                    };
                }
                else if (current != null && !string.IsNullOrWhiteSpace(t))
                {
                    current.Text += (current.Text.Length > 0 ? " " : "") + t;
                }
            }
            if (current != null)
            {
                result.Add(current);
            }

            return result;
        }

        private static List<string> ParseHighlights(List<string> lines)
        {
            return lines
                .Select(l => l.Trim())
                .Where(l => l.StartsWith("- ") || l.StartsWith("* "))
                .Select(l => l.Substring(2).Trim())
                .Where(l => l.Length > 0)
                .ToList();
        }

        private static List<SpecRow> ParseSpecs(List<string> lines)
        {
            var result = new List<SpecRow>();
            int autoNum = 1;

            foreach (var line in lines)
            {
                var t = line.Trim();
                if (string.IsNullOrEmpty(t) || t.StartsWith("#"))
                {
                    continue;
                }

                var parts = t.Split('|').Select(p => p.Trim()).ToArray();
                if (parts.Length == 3)
                {
                    result.Add(new SpecRow { Number = parts[0], Param = parts[1], Value = parts[2] });
                }
                else if (parts.Length == 2)
                {
                    result.Add(new SpecRow
                    {
                        Number = autoNum.ToString(),
                        Param = parts[0],
                        Value = parts[1]
                    });
                    autoNum++;
                }
            }
            return result;
        }

        private static List<UseCase> ParseUseCases(List<string> lines)
        {
            var result = new List<UseCase>();
            UseCase? current = null;

            foreach (var line in lines)
            {
                var t = line.Trim();
                if (t.StartsWith("### "))
                {
                    if (current != null)
                    {
                        result.Add(current);
                    }

                    current = new UseCase { Title = t.Substring(4).Trim() };
                }
                else if (current != null && !string.IsNullOrWhiteSpace(t))
                {
                    current.Text += (current.Text.Length > 0 ? " " : "") + t;
                }
            }
            if (current != null)
            {
                result.Add(current);
            }

            return result;
        }
    }
}