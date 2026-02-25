using Website_Progress.DataContext;
using Website_Progress.Interfaces;
using Website_Progress.ModelsDTO;


namespace Website_Progress.Repositories
{

    public class ProductsDbRepository : IProductRepository
    {
        private readonly DatabaseContext _databaseContext;

        public ProductsDbRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public List<Product> GetAll() => _databaseContext.Products.ToList();

        public Product? TryGetById(int productId) =>
            _databaseContext.Products.FirstOrDefault(product => product.Id == productId);

        public void Add(Product product)
        {
            _databaseContext.Products.Add(product);

            _databaseContext.SaveChanges();  // Сохраняем изменения в БД
        }

        public void Delete(int productId)
        {
            var existingProduct = TryGetById(productId);

            if (existingProduct != null)
            {
                _databaseContext.Products.Remove(existingProduct);
                _databaseContext.SaveChanges();  // Сохраняем изменения в БД
            }
        }

        public void Update(Product product)
        {
            var excitingProduct = TryGetById(product.Id);

            if (excitingProduct != null)
            {
                excitingProduct.Name = product.Name;
                excitingProduct.Cost = product.Cost;
                excitingProduct.Description = product.Description;

                _databaseContext.SaveChanges();  // Сохраняем изменения в БД
            }
        }

        public List<Product> Search(string text)
        {
            var products = GetAll().Where(product => product.Name!.Contains(text, StringComparison.CurrentCultureIgnoreCase));

            return products.ToList() ?? [];
        }
    }
}
