using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Website_Progress.Helpers;
using Website_Progress.Models;

namespace Website_Progress.Areas.Admin.Controllers
{
    [Area(Constants.AdminRoleName)]
    [Authorize(Roles = Constants.AdminRoleName)]

    public class OrderController(IOrderRepository ordersRepository) : Controller
    {
        private readonly IOrderRepository _ordersRepository = ordersRepository;

        public async Task<IActionResult> Index()
        {
            var orders = await _ordersRepository.GetAllAsync();
            return View(orders.ToOrderViewModels());
        }

        public async Task<IActionResult> Detail(Guid orderId)
        {
            var order = await _ordersRepository.TryGetByIdAsync(orderId);
            return View(order?.ToOrderViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(Guid orderId, OrderStatusViewModel status)
        {
            await _ordersRepository.UpdateStatusAsync(orderId, (OrderStatus)status);
            return RedirectToAction(nameof(Index));
        }
    }
}
