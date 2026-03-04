using Website_Progress.ModelsDTO;

namespace Website_Progress.Interfaces
{
    public interface INewsRepository
    {
        Task<List<News>> GetAllAsync();
        Task<News?> TryGetByIdAsync(int id);
        Task AddAsync(News news);
        Task DeleteAsync(int id);
        Task UpdateAsync(News news);
        Task<List<News>> GetForMainPageAsync();
    }
}
