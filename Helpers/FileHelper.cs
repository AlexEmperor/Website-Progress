namespace Website_Progress.Helpers
{
    public static class FileHelper
    {
        public static async Task<byte[]?> ToBytesAsync(IFormFile? file)
        {
            if (file == null)
            {
                return null;
            }

            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            return ms.ToArray();
        }
    }
}
