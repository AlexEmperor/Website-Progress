using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Website_Progress.Helpers;
using Website_Progress.Interfaces;

namespace Website_Progress.Areas.User.Controllers
{
    [Area(Constants.UserRoleName)]
    [Authorize(Roles = Constants.UserRoleName)]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _ordersRepository;
        public OrderController(IOrderRepository ordersRepository)
        {
            _ordersRepository = ordersRepository;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized();
            }

            var orders = await _ordersRepository.TryGetAllOrdersByUserIdAsync(userId);

            return View(orders.ToOrderViewModels());
        }
    }
}
