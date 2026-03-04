using Microsoft.AspNetCore.Mvc;
using Website_Progress.Helpers;
using Website_Progress.Interfaces;

namespace Website_Progress.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductRepository _productRepository;

        public CatalogController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllAsync();

            var ordered = products
                .OrderBy(p => p.Id)
                .ToList();

            return View(ordered.ToProductViewModels());
        }
    }
}
