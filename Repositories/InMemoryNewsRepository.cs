using Website_Progress.Interfaces;
using Website_Progress.Models;

namespace Website_Progress.Repositories
{
    public class InMemoryNewsRepository : INewsRepository
    {
        private int _instanceCounter = 0;

        private readonly List<NewsViewModel> _news;

        public InMemoryNewsRepository()
        {
            _news =
            [
                new NewsViewModel(++_instanceCounter, "Новость 1", "Описание новости 1", "/img/product2.png", DateOnly.MaxValue),
                new NewsViewModel(++_instanceCounter, "Новость 2", "Описание новости 2", "/img/product3.png", DateOnly.MaxValue),
                new NewsViewModel(++_instanceCounter, "Новость 3", "Описание новости 3", "/img/product4.png", DateOnly.MaxValue),
                new NewsViewModel(++_instanceCounter, "Новость 4", "Описание новости 4", "/img/product2.png", DateOnly.MaxValue)
            ];
        }

        public List<NewsViewModel> GetAll() => _news;

        public NewsViewModel? TryGetById(int id) => _news.FirstOrDefault(product => product.Id == id);

    }
}
