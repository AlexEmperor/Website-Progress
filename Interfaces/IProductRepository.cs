using Website_Progress.ModelsDTO;

namespace Website_Progress.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();
        Task<Product?> TryGetByIdAsync(int productId);
        Task AddAsync(Product product);
        Task DeleteAsync(int productId);
        Task UpdateAsync(Product product);
        Task<List<Product>> SearchAsync(string text);
        Task<List<Product>> GetForMainPageAsync();
    }
}
