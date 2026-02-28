namespace Website_Progress.Helpers
{
    public static class FileSaver
    {
        public static async Task<string?> SaveFileAsync(IFormFile? file, string folderName, IWebHostEnvironment _environment)
        {
            if (file == null)
            {
                return null;
            }

            string uploadsFolder = Path.Combine(_environment.WebRootPath, folderName);
            Directory.CreateDirectory(uploadsFolder);

            string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return "/" + folderName + "/" + fileName;
        }
    }
}
