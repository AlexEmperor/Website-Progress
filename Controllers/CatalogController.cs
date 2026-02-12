using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Index()
        {

            return View(_productRepository.GetAll());

            // var products = _productRepository.GetAll();
            //return View(products.ToProductViewModels());
            ////return View(_productRepository.GetAll());
        }
    }
}
