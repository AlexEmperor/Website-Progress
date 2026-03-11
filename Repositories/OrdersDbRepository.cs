using Microsoft.EntityFrameworkCore;
using Website_Progress.DataContext;
using Website_Progress.Interfaces;
using Website_Progress.ModelsDTO;

namespace Website_Progress.Repositories
{
    public class OrdersDbRepository : IOrderRepository
    {
        private readonly DatabaseContext _databaseContext;

        public OrdersDbRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<Order?> TryGetOrderByUserIdAsync(Guid userId)
        {
            return await _databaseContext.Orders
                .Include(x => x.DeliveryUser)
                .FirstOrDefaultAsync(x => x.DeliveryUser.Id == userId);
        }
        public async Task<List<Order>> TryGetAllOrdersByUserIdAsync(string userId)
        {
            return await _databaseContext.Orders
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .Include(x => x.DeliveryUser)
                .Include(x => x.Items)
                    .ThenInclude(x => x.Product)
                .OrderByDescending(x => x.CreationDateTime)
                .ToListAsync();
        }

        public async Task AddAsync(Order order)
        {
            order.Id = Guid.NewGuid();
            order.CreationDateTime = DateTime.Now;
            order.DeliveryUser.Id = Guid.NewGuid();
            order.Status = OrderStatus.Created;

            await _databaseContext.Orders.AddAsync(order);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await _databaseContext.Orders
                .AsNoTracking()
                .Include(x => x.DeliveryUser)
                .Include(x => x.Items)
                    .ThenInclude(x => x.Product)
                .OrderByDescending(x => x.CreationDateTime)
                .ToListAsync();
        }

        public async Task<Order?> TryGetByIdAsync(Guid orderId)
        {
            return await _databaseContext.Orders
                .AsNoTracking()
                .Include(x => x.DeliveryUser)
                .Include(x => x.Items)
                    .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(order => order.Id == orderId);
        }

        public async Task UpdateStatusAsync(Guid orderId, OrderStatus newStatus)
        {
            var existingOrder = await _databaseContext.Orders
                .FirstOrDefaultAsync(x => x.Id == orderId);

            if (existingOrder != null)
            {
                existingOrder.Status = newStatus;
                await _databaseContext.SaveChangesAsync();
            }
        }


    }
}
