using Website_Progress.Interfaces;
using Website_Progress.Models;


namespace WEBtest.Repositories
{

    public class InMemoryProductRepository : IProductRepository
    {
        private int _instanceCounter = 0;

        private readonly List<ProductViewModel> _products;

        public InMemoryProductRepository()
        {
            _products =
            [
                new ProductViewModel(++_instanceCounter, "Товар 1", 1000.0M, "Описание товара 1", "/img/product2.png"),
                new ProductViewModel(++_instanceCounter, "Товар 2", 2000.0M, "Описание товара 2", "/img/product3.png"),
                new ProductViewModel(++_instanceCounter, "Товар 3", 3000.0M, "Описание товара 3", "/img/product4.png"),
                new ProductViewModel(++_instanceCounter, "Товар 4", 4000.0M, "Описание товара 4", "/img/product2.png"),
                new ProductViewModel(++_instanceCounter, "Товар 5", 5000.0M, "Описание товара 5", "/img/product3.png"),
                new ProductViewModel(++_instanceCounter, "Товар 6", 6000.0M, "Описание товара 6", "/img/product4.png"),
                new ProductViewModel(++_instanceCounter, "Товар 7", 7000.0M, "Описание товара 7", "/img/product2.png"),
                new ProductViewModel(++_instanceCounter, "Товар 8", 8000.0M, "Описание товара 8", "/img/product3.png"),
                new ProductViewModel(++_instanceCounter, "Товар 9", 9000.0M, "Описание товара 9", "/img/product4.png"),
                new ProductViewModel(++_instanceCounter, "Товар 10", 10000.0M, "Описание товара 10", "/img/product2.png")
            ];
        }

        public List<ProductViewModel> GetAll() => _products;
        public List<ProductViewModel> Search(string text)
        {
            var products = GetAll().Where(product => product.Name!.Contains(text, StringComparison.OrdinalIgnoreCase));

            return products.ToList() ?? [];
        }

        public ProductViewModel? TryGetById(int id) => _products.FirstOrDefault(product => product.Id == id);

        public void Add(string name, decimal cost, string description, string? photoPath)
        {
            var product = new ProductViewModel(++_instanceCounter, name, cost, description, photoPath);

            _products.Add(product);
        }
        public void Add(ProductViewModel product)
        {
            product.Id = ++_instanceCounter;

            _products.Add(product);
        }
        public void Delete(int productId)
        {
            var existingProduct = TryGetById(productId);

            if (existingProduct != null)
            {
                _products.Remove(existingProduct);
            }
        }
        public void Update(ProductViewModel product)
        {
            var excitingProduct = TryGetById(product.Id);

            if (excitingProduct != null)
            {
                excitingProduct.Name = product.Name;
                excitingProduct.Cost = product.Cost;
                excitingProduct.Description = product.Description;
            }
        }
    }
}
