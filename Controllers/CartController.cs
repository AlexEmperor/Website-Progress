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

        public async Task<IActionResult> Index()
        {
            var cart = await _cartRepository.TryGetByUserIdAsync(GetUserId());
            return View(cart.ToCartViewModel());
        }

        [Authorize]
        public async Task<IActionResult> Add(int productId)
        {
            var product = await _productRepository.TryGetByIdAsync(productId);

            if (product != null)
            {
                await _cartRepository.AddAsync(product, GetUserId());
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int productId)
        {
            await _cartRepository.DeleteAsync(productId, GetUserId());
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Clear()
        {
            await _cartRepository.ClearAsync(GetUserId());
            return RedirectToAction(nameof(Index));
        }

    }
}
