using Website_Progress.ModelsDTO;

namespace Website_Progress.Interfaces
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllOrdersByUserIdAsync(string userId);
        Task AddAsync(Order order);
        Task<List<Order>> GetAllAsync();
        Task<Order?> TryGetByIdAsync(Guid orderId);
        Task UpdateStatusAsync(Guid orderId, OrderStatus status);
    }
}
