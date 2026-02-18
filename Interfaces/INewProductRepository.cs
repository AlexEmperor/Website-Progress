using Website_Progress.Models;

namespace Website_Progress.Interfaces
{
    public interface INewProductRepository
    {
        void Add(string name, decimal cost, string description, string? photoPath, string? presentationPath);
        List<NewProductViewModel> GetAll();
        List<NewProductViewModel> Search(string text);
        NewProductViewModel? TryGetById(int id);
        void Add(NewProductViewModel product);
        void Delete(int productId);
        void Update(NewProductViewModel product);
    }
}
