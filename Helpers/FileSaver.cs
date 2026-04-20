public static class FileSaver
{
    public static async Task<string?> SaveFileAsync(
        IFormFile? file,
        string folderName,
        IWebHostEnvironment _environment,
        string? baseName = null)
    {
        if (file == null)
        {
            return null;
        }

        string uploadsFolder = Path.Combine(_environment.WebRootPath, folderName);
        Directory.CreateDirectory(uploadsFolder);

        string safeBaseName = string.IsNullOrEmpty(baseName)
            ? "file"
            : baseName.Replace(" ", "-").ToLower();

        string extension = Path.GetExtension(file.FileName);

        // Уникальный суффикс — чтобы файлы не перезаписывали друг друга
        string uniqueSuffix = Guid.NewGuid().ToString("N").Substring(0, 8);
        string fileName = $"{safeBaseName}_{uniqueSuffix}{extension}";

        string filePath = Path.Combine(uploadsFolder, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return "/" + folderName + "/" + fileName;
    }
}