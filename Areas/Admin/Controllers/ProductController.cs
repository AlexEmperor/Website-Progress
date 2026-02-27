using Microsoft.AspNetCore.Mvc;
using Website_Progress.Helpers;
using Website_Progress.Interfaces;
using Website_Progress.Models;

namespace WEBtest.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productsRepository;


        public ProductController(IProductRepository productsRepository)
        {
            _productsRepository = productsRepository;

        }


        public IActionResult Index()
        {
            var products = _productsRepository.GetAll();
            return View(products.ToProductViewModels());
            //return View(products);
        }


        public IActionResult Add()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Add(ProductViewModel product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            _productsRepository.Add(product.ToProductDb());
            //_productsRepository.Add(product);

            return RedirectToAction(nameof(Index));
        }


        public IActionResult Delete(int id)
        {
            _productsRepository.Delete(id);

            return RedirectToAction(nameof(Index));
        }


        public IActionResult Update(int id)
        {
            var existingProduct = _productsRepository.TryGetById(id);
            return View(existingProduct?.ToProductViewModel());
            //return View(existingProduct);
        }


        [HttpPost]
        public IActionResult Update(ProductViewModel product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            _productsRepository.Update(product.ToProductDb());
            //_productsRepository.Update(product);

            return RedirectToAction(nameof(Index));
        }
    }
}
