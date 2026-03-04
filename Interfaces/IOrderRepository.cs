using Website_Progress.ModelsDTO;

namespace Website_Progress.Interfaces
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order);
        Task<List<Order>> GetAllAsync();
        Task<Order?> TryGetByIdAsync(Guid orderId);
        Task UpdateStatusAsync(Guid orderId, OrderStatus status);
    }
}
