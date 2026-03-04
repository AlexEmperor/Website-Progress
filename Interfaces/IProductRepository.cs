using Website_Progress.ModelsDTO;

namespace Website_Progress.Interfaces
{
    public interface IProductRepository
    {
        List<Product> GetAll();
        Product? TryGetById(int productId);
        void Add(Product product);
        void Delete(int productId);
        void Update(Product product);
        List<Product> Search(string text);
        List<Product> GetForMainPage();
    }
}
