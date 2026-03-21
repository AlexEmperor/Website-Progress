namespace Website_Progress.Repositories
{

    public class ProductsDbRepository : IProductRepository
    {
        private readonly DatabaseContext _databaseContext;

        public ProductsDbRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _databaseContext.Products.AsNoTracking().ToListAsync();
        }

        public async Task<Product?> TryGetByIdAsync(int productId)
        {
            return await _databaseContext.Products
                .FirstOrDefaultAsync(product => product.Id == productId);
        }

        public async Task AddAsync(Product product)
        {
            await _databaseContext.Products.AddAsync(product);
            await _databaseContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int productId)
        {
            var existingProduct = await TryGetByIdAsync(productId);

            if (existingProduct != null)
            {
                _databaseContext.Products.Remove(existingProduct);
                await _databaseContext.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(Product product)
        {
            var existingProduct = await TryGetByIdAsync(product.Id);

            if (existingProduct != null)
            {
                existingProduct.Name = product.Name;
                existingProduct.Cost = product.Cost;
                existingProduct.Description = product.Description;
                existingProduct.ShortDescription = product.ShortDescription;
                existingProduct.IsOnMainPage = product.IsOnMainPage;

                await _databaseContext.SaveChangesAsync();
            }
        }

        public async Task<List<Product>> SearchAsync(string text)
        {
            return await _databaseContext.Products
                .Where(product =>
                    product.Name != null &&
                    product.Name.Contains(text))
                .ToListAsync();
        }

        public async Task<List<Product>> GetForMainPageAsync()
        {
            return await _databaseContext.Products
                .Where(x => x.IsOnMainPage)
                .OrderByDescending(x => x.Id)
                .Take(3)
                .ToListAsync();
        }
    }
}
