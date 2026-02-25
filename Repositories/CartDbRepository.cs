using Microsoft.EntityFrameworkCore;
using Website_Progress.DataContext;
using Website_Progress.Interfaces;
using Website_Progress.ModelsDTO;

namespace Website_Progress.Repositories
{
    public class CartDbRepository : ICartRepository
    {
        private readonly List<Cart> _carts = [];

        private readonly DatabaseContext _databaseContext;

        public CartDbRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public Cart? TryGetByUserId(string userId)
        {
            return _databaseContext.Carts.Include(x => x.Items)
                .ThenInclude(x => x.Product).FirstOrDefault(x => x.UserId == userId);
        }

        public void Add(Product product, string userId)
        {
            var existingCart = TryGetByUserId(userId);

            if (existingCart == null)
            {
                existingCart = new Cart()
                {
                    UserId = userId,
                    Items = [],
                    CreationDateTime = DateTime.UtcNow
                };

                existingCart.Items =
                    [
                        new CartItem()
                        {
                            Product = product,
                            Quantity = 1,
                            Cart = existingCart
                        }

                ];
                _databaseContext.Carts.Add(existingCart);
            }
            else
            {
                var existingCartItem = existingCart.Items
                    .FirstOrDefault(item => item.Product.Id == product.Id);

                if (existingCartItem == null)
                {
                    var newCartItem = new CartItem()
                    {
                        Product = product,
                        Quantity = 1,
                        Cart = existingCart
                    };
                    existingCart.Items.Add(newCartItem);
                }
                else
                {
                    existingCartItem.Quantity++;
                }
            }

            _databaseContext.SaveChanges();  // Сохраняем изменения в БД
        }

        public void Delete(int productId, string userId)
        {
            var existingCart = TryGetByUserId(userId);

            var existingCartItem = existingCart?.Items
                .FirstOrDefault(item => item.Product.Id == productId);

            if (existingCartItem == null)
            {
                return;
            }

            existingCartItem.Quantity--;

            if (existingCartItem.Quantity == 0)
            {
                existingCart?.Items.Remove(existingCartItem);
            }

            _databaseContext.SaveChanges();  // Сохраняем изменения в БД
        }

        public void Clear(string userId)
        {
            var existingCart = TryGetByUserId(userId);

            if (existingCart != null)
            {
                _databaseContext.Carts.Remove(existingCart);

                _databaseContext.SaveChanges();  // Сохраняем изменения в БД
            }
        }
    }
}
