using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using System.Security.Claims;
using Website_Progress.Helpers;
using Website_Progress.Interfaces;
using Website_Progress.Services;

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
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId is null)
            {
                return Unauthorized();
            }

            var orders = await _ordersRepository.GetAllOrdersByUserIdAsync(userId);

            return View(orders.ToOrderViewModels());
        }

        public async Task<IActionResult> GenerateInvoice(Guid id)
        {
            var order = await _ordersRepository.TryGetByIdAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            var document = new InvoiceDocument(order);

            var pdf = document.GeneratePdf();

            return File(pdf, "application/pdf", $"Invoice_{order.Id}.pdf");
        }


        public async Task<IActionResult> Detail(Guid orderId)
        {
            var order = await _ordersRepository.TryGetByIdAsync(orderId);
            return View(order?.ToOrderViewModel());
        }
    }
}
