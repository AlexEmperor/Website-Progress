using Microsoft.AspNetCore.Mvc;
using Website_Progress.Helpers;
using Website_Progress.Interfaces;

namespace Website_Progress.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICartRepository _cartRepository;

        public CartController(IProductRepository productRepository, ICartRepository cartRepository)
        {
            _productRepository = productRepository;
            _cartRepository = cartRepository;
        }

        public IActionResult Index()
        {
            var cart = _cartRepository.TryGetByUserId(Constants.UserId);

            return View(cart.ToCartViewModel());
            //return View(cart.ToCartViewModel());
        }

        public IActionResult Add(int productId)
        {
            _cartRepository.Add(_productRepository.TryGetById(productId), Constants.UserId);

            return RedirectToAction(nameof(Index));
            //return View("../Home/index", ProductRepository.GetAll());
        }

        public IActionResult Delete(int productId)
        {
            _cartRepository.Delete(productId, Constants.UserId);

            //_cartRepository.Delete(productId/*_productRepository.TryGetById(productId)*/, Constants.UserId);

            return RedirectToAction(nameof(Index));
            //return View("../Home/index", ProductRepository.GetAll());
        }
        public IActionResult Clear()
        {
            _cartRepository.Clear(Constants.UserId);

            return RedirectToAction(nameof(Index));
        }

    }
}
