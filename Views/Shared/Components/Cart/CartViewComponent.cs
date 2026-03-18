using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Website_Progress.Helpers;
using Website_Progress.Interfaces;

namespace Website_Progress.Views.Shared.Components.Cart
{
    public class CartViewComponent : ViewComponent
    {
        private readonly ICartRepository _cartRepository;

        public CartViewComponent(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        private string GetUserId()
        {
            return HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var cart = await _cartRepository.TryGetByUserIdAsync(GetUserId());
            var productsCount = cart?.ToCartViewModel()?.Quantity ?? 0;

            return View("Cart", productsCount);
        }
    }
}
