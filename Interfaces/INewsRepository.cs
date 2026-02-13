using Website_Progress.Models;

namespace Website_Progress.Interfaces
{
    public interface INewsRepository
    {
        List<NewsViewModel> GetAll();
        //List<NewsViewModel> Search(string text);
        NewsViewModel? TryGetById(int id);
    }
}
