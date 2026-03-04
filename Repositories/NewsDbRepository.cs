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

        public List<News> GetAll() => _databaseContext.News.ToList();

        public News? TryGetById(int id) => _databaseContext.News.FirstOrDefault(product => product.Id == id);

        public void Add(News product)
        {
            _databaseContext.News.Add(product);

            _databaseContext.SaveChanges();  // Сохраняем изменения в БД
        }

        public void Delete(int productId)
        {
            var existingProduct = TryGetById(productId);

            if (existingProduct != null)
            {
                _databaseContext.News.Remove(existingProduct);
                _databaseContext.SaveChanges();  // Сохраняем изменения в БД
            }
        }

        public void Update(News news)
        {
            var existingNews = TryGetById(news.Id);

            if (existingNews != null)
            {
                existingNews.Title = news.Title;
                existingNews.ShortText = news.ShortText;
                // existingNews.Description = news.Description;

                _databaseContext.SaveChanges();  // Сохраняем изменения в БД
            }
        }
    }
}
