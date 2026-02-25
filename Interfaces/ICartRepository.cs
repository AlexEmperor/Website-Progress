using Website_Progress.ModelsDTO;

namespace Website_Progress.Interfaces
{
    public interface ICartRepository
    {
        void Add(Product product, string userId);
        void Delete(int productId, string userId);
        void Clear(string userId);
        Cart? TryGetByUserId(string userId);
    }
}
