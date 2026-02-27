using Microsoft.AspNetCore.Mvc;
using Website_Progress.Helpers;
using Website_Progress.Interfaces;
using Website_Progress.Models;
using Website_Progress.ModelsDTO;

namespace Website_Progress.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _ordersRepository;


        public OrderController(IOrderRepository ordersRepository)
        {
            _ordersRepository = ordersRepository;

        }


        public IActionResult Index()
        {
            var orders = _ordersRepository.GetAll();

            return View(orders.ToOrderViewModels());
        }


        public IActionResult Detail(Guid orderId)
        {
            var order = _ordersRepository.TryGetById(orderId);

            return View(order?.ToOrderViewModel());
        }


        [HttpPost]
        public IActionResult UpdateStatus(Guid orderId, OrderStatusViewModel status)
        {
            _ordersRepository.UpdateStatus(orderId, (OrderStatus)status);

            return RedirectToAction(nameof(Index));
        }
    }
}
