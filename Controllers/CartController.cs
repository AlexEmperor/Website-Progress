using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public IActionResult Index()
        {
            var cart = _cartRepository.TryGetByUserId(GetUserId());

            return View(cart.ToCartViewModel());
        }

        [Authorize]
        public IActionResult Add(int productId)
        {
            _cartRepository.Add(_productRepository.TryGetById(productId), GetUserId());

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int productId)
        {
            _cartRepository.Delete(productId, GetUserId());

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Clear()
        {
            _cartRepository.Clear(GetUserId());

            return RedirectToAction(nameof(Index));
        }

    }
}
