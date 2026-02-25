using Website_Progress.ModelsDTO;

namespace Website_Progress.Interfaces
{
    public interface INewsRepository
    {
        List<News> GetAll();
        //List<NewsViewModel> Search(string text);
        News? TryGetById(int id);
    }
}
