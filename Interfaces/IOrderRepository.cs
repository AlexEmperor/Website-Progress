using Website_Progress.ModelsDTO;

namespace Website_Progress.Interfaces
{
    public interface IOrderRepository
    {
        //Task<Order?> TryGetOrderByUserIdAsync(string userId);
        Task<List<Order>> TryGetAllOrdersByUserIdAsync(string userId);

        Task AddAsync(Order order);
        Task<List<Order>> GetAllAsync();
        Task<Order?> TryGetByIdAsync(Guid orderId);
        Task UpdateStatusAsync(Guid orderId, OrderStatus status);
    }
}
