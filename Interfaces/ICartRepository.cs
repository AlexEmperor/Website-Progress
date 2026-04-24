namespace Website_Progress.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart?> TryGetByUserIdAsync(string userId);
        Task AddAsync(Product product, string userId);
        Task DeleteAsync(int productId, string userId);
        Task ClearAsync(string userId);
    }
}
