namespace Website_Progress.Helpers
{
    public interface IFileStorage
    {
        Task<string?> SaveAsync(IFormFile? file, string folder, string? baseName = null);
        Task DeleteAsync(string? publicUrl);
    }
}
