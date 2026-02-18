using Website_Progress.Interfaces;
using Website_Progress.Models;

namespace Website_Progress.Repositories
{
    public class InMemoryNewProductRepository : INewProductRepository
    {
        private int _instanceCounter = 0;

        private readonly List<NewProductViewModel> _products;

        public InMemoryNewProductRepository()
        {
            _products =
            [
                new NewProductViewModel(++_instanceCounter, "АК «SDR»",
                1000.0M, "Устройство сканирования частотного спектра.", "/img/product2.png", "/presentations/КП Интеллект АК SDR(3).pptx"),
                new NewProductViewModel(++_instanceCounter, "Аппаратный модуль VVizor",
                2000.0M, "Устройство фиксирования видео передатчиков.", "/img/product3.png", "/presentations/АМ_VVizor_HiddenParts.pptx"),
                new NewProductViewModel(++_instanceCounter, "МЭМС",
                3000.0M, "Устройство позиционирования.", "/img/product4.png", "")
            ];
        }
        public List<NewProductViewModel> GetAll() => _products;

        public List<NewProductViewModel> Search(string text)
        {
            var products = GetAll().Where(product => product.Name!.Contains(text, StringComparison.OrdinalIgnoreCase));

            return products.ToList() ?? [];
        }

        public NewProductViewModel? TryGetById(int id) => _products.FirstOrDefault(product => product.Id == id);

        public void Add(string name, decimal cost, string description, string? photoPath, string? presentationPath)
        {
            var product = new NewProductViewModel(++_instanceCounter, name, cost, description, photoPath, presentationPath);

            _products.Add(product);
        }
        public void Add(NewProductViewModel product)
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
        public void Update(NewProductViewModel product)
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
