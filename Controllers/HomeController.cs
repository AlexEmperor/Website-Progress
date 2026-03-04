using Microsoft.AspNetCore.Mvc;
using Website_Progress.Helpers;
using Website_Progress.Interfaces;
using Website_Progress.Models;

namespace Website_Progress.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly INewsRepository _newsRepository;

        public HomeController(IProductRepository productRepository, INewsRepository newsRepository)
        {
            _productRepository = productRepository;
            _newsRepository = newsRepository;
        }

        public IActionResult Index()
        {
            var model = new HomeViewModel
            {
                News = _newsRepository
                            .GetForMainPage()
                            .ToNewsViewModels(),

                FeaturedProducts = _productRepository
                            .GetForMainPage()
                            .ToProductViewModels()
            };

            return View(model);
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
