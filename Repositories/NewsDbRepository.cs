using Microsoft.EntityFrameworkCore;
using Website_Progress.DataContext;
using Website_Progress.Interfaces;
using Website_Progress.ModelsDTO;

namespace Website_Progress.Repositories
{
    public class NewsDbRepository : INewsRepository
    {
        private readonly DatabaseContext _databaseContext;

        public NewsDbRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<List<News>> GetAllAsync()
               => await _databaseContext.News.ToListAsync();

        public async Task<News?> TryGetByIdAsync(int id)
            => await _databaseContext.News
                .FirstOrDefaultAsync(x => x.Id == id);

        public async Task AddAsync(News news)
        {
            await _databaseContext.News.AddAsync(news);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await TryGetByIdAsync(id);
            if (existing != null)
            {
                _databaseContext.News.Remove(existing);
                await _databaseContext.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(News news)
        {
            var existing = await TryGetByIdAsync(news.Id);
            if (existing != null)
            {
                existing.Title = news.Title;
                existing.ShortText = news.ShortText;
                existing.ImageUrl = news.ImageUrl;
                existing.IsOnMainPage = news.IsOnMainPage;

                await _databaseContext.SaveChangesAsync();
            }
        }

        public async Task<List<News>> GetForMainPageAsync()
        {
            return await _databaseContext.News
                .Where(x => x.IsOnMainPage)
                .OrderByDescending(x => x.Date)
                .Take(3)
                .ToListAsync();
        }
    }
}
