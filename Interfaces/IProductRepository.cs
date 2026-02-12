using Website_Progress.Models;

namespace Website_Progress.Interfaces
{
    public interface IProductRepository
    {
        void Add(string name, decimal cost, string description, string? photoPath);
        List<ProductViewModel> GetAll();
        List<ProductViewModel> Search(string text);
        ProductViewModel? TryGetById(int id);
        void Add(ProductViewModel product);
        void Delete(int productId);
        void Update(ProductViewModel product);
    }
}
