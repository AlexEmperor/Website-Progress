using Microsoft.AspNetCore.Mvc;
using Website_Progress.Interfaces;

namespace Website_Progress.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IActionResult Index(int id)
        {
            var product = _productRepository.TryGetById(id);
            //return View(product?.ToProductViewModel());
            return View(product);
        }
    }
}

