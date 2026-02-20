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
                new ProductViewModel(++_instanceCounter, "АК «SDR»", 1000.0M, "Устройство сканирования частотного спектра.", "/img/product2.png", "/presentations/КП Интеллект АК SDR(3).pptx"),
                new ProductViewModel(++_instanceCounter, "VVizor", 2000.0M, "Устройство фиксирования видео передатчиков.", "/img/product3.png", "/presentations/АМ_VVizor_HiddenParts.pptx"),
                new ProductViewModel(++_instanceCounter, "МЭМС", 3000.0M, "Устройство позиционирования.", "/img/product4.png", ""),
                new ProductViewModel(++_instanceCounter, "Ретранслятор", 4000.0M, "Плата ретранслятора с программным обеспечением автоматизированного тестирования выпускаемых изделий.", "/img/product2.png", ""),
                new ProductViewModel(++_instanceCounter, "Радиосканер", 5000.0M, "Двухканальное устройство сканирования частотного спектра.", "/img/product3.png", ""),
                new ProductViewModel(++_instanceCounter, "Bluetooth-маяк", 6000.0M, "Описание товара 6", "/img/product4.png", ""),
                new ProductViewModel(++_instanceCounter, "Радиомаяк", 7000.0M, "Описание товара 7", "/img/product2.png", ""),
                new ProductViewModel(++_instanceCounter, "Товар 8", 8000.0M, "Описание товара 8", "/img/product3.png", ""),
                new ProductViewModel(++_instanceCounter, "Товар 9", 9000.0M, "Описание товара 9", "/img/product4.png", ""),
                new ProductViewModel(++_instanceCounter, "Товар 10", 10000.0M, "Описание товара 10", "/img/product2.png", "")
            ];
        }

        public List<ProductViewModel> GetAll() => _products;

        public List<ProductViewModel> Search(string text)
        {
            var products = GetAll().Where(product => product.Name!.Contains(text, StringComparison.OrdinalIgnoreCase));

            return products.ToList() ?? [];
        }

        public ProductViewModel? TryGetById(int id) => _products.FirstOrDefault(product => product.Id == id);

        public void Add(string name, decimal cost, string description, string? photoPath, string? presentationPath)
        {
            var product = new ProductViewModel(++_instanceCounter, name, cost, description, photoPath, presentationPath);

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
