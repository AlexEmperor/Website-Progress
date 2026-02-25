using Website_Progress.DataContext;
using Website_Progress.Interfaces;
using Website_Progress.Models;
using Website_Progress.ModelsDTO;

namespace Website_Progress.Repositories
{
    public class NewsDbRepository : INewsRepository
    {
        private int _instanceCounter = 0;

        private readonly List<NewsViewModel> _news;

        private readonly DatabaseContext _databaseContext;

        public NewsDbRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }


        public List<News> GetAll() => _databaseContext.News.ToList();

        public News? TryGetById(int id) => _databaseContext.News.FirstOrDefault(product => product.Id == id);

    }
}
