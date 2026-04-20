using Microsoft.AspNetCore.Mvc;
using Website_Progress.Helpers;
using Website_Progress.Models;

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
            if (product == null)
            {
                return View((ProductViewModel?)null);
            }

            var vm = product.ToProductViewModel();

            // Парсим структурированный контент из Description
            var content = ProductDescriptionParser.Parse(vm.Description);
            ViewData["Content"] = content;

            return View(vm);
        }
        //public async Task<IActionResult> Index(int id)
        //{
        //    var product = await _productRepository.TryGetByIdAsync(id);

        //    return View(product?.ToProductViewModel());
        //}
    }
}

