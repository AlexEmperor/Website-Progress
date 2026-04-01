namespace Website_Progress.Repositories
{
    public class CartDbRepository : ICartRepository
    {
        private readonly DatabaseContext _databaseContext;

        public CartDbRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<Cart?> TryGetByUserIdAsync(string userId)
        {
            return await _databaseContext.Carts
                .Include(x => x.Items)
                    .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task AddAsync(Product product, string userId)
        {
            var existingCart = await TryGetByUserIdAsync(userId);

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
                            Cart = existingCart,
                            PriceAtPurchase = product.Cost
                        }

                ];
                await _databaseContext.Carts.AddAsync(existingCart);
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
                        Cart = existingCart,
                        PriceAtPurchase = product.Cost
                    };
                    existingCart.Items.Add(newCartItem);
                }
                else
                {
                    existingCartItem.Quantity++;
                }
            }

            await _databaseContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int productId, string userId)
        {
            var existingCart = await TryGetByUserIdAsync(userId);

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

            await _databaseContext.SaveChangesAsync();
        }

        public async Task ClearAsync(string userId)
        {
            var existingCart = await TryGetByUserIdAsync(userId);

            if (existingCart != null)
            {
                _databaseContext.Carts.Remove(existingCart);
                await _databaseContext.SaveChangesAsync();
            }
        }
    }
}
