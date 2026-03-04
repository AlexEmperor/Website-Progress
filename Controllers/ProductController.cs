using Microsoft.AspNetCore.Mvc;
using Website_Progress.Helpers;
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

        public async Task<IActionResult> Index(int id)
        {
            var product = await _productRepository.TryGetByIdAsync(id);

            return View(product?.ToProductViewModel());
        }
    }
}

