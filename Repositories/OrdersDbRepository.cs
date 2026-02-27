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

        public void Add(Order order)
        {
            order.Id = Guid.NewGuid();
            order.CreationDateTime = DateTime.Now; // -3 часа от Москвы
            order.DeliveryUser.Id = Guid.NewGuid();
            order.Status = OrderStatus.Created;

            _databaseContext.Orders.Add(order);

            _databaseContext.SaveChanges();
        }

        public List<Order> GetAll() => _databaseContext.Orders
            .Include(x => x.DeliveryUser)
            .Include(x => x.Items)
            .ThenInclude(x => x.Product)
            .OrderByDescending(x => x.CreationDateTime)
            .ToList();

        public Order? TryGetById(Guid orderId) =>
            _databaseContext.Orders
            .Include(x => x.DeliveryUser)
            .Include(x => x.Items)
            .ThenInclude(x => x.Product)
            .FirstOrDefault(order => order.Id == orderId);

        public void UpdateStatus(Guid orderId, OrderStatus newStatus)
        {
            var existingOrder = TryGetById(orderId);

            if (existingOrder != null)
            {
                existingOrder.Status = newStatus;

                _databaseContext.SaveChanges();
            }
        }
    }
}
