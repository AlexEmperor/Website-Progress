using Microsoft.AspNetCore.Mvc;
using Website_Progress.Interfaces;

namespace Website_Progress.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepository _productRepository;

        public HomeController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Search(string query)
        {
            if (query == null)
            {
                return View();
            }
            // return View(); //заглушка

            var products = _productRepository.Search(query);

            return View(products/*.ToProductViewModels()*/);

            /*var products = _productRepository.Search(query);

            return View(products);*/
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
