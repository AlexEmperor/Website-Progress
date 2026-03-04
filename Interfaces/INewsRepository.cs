using Website_Progress.ModelsDTO;

namespace Website_Progress.Interfaces
{
    public interface INewsRepository
    {
        List<News> GetAll();
        void Add(News product);
        void Delete(int productId);
        void Update(News product);

        List<News> GetForMainPage();
        News? TryGetById(int id);
    }
}
