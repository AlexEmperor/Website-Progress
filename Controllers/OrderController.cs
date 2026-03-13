using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using System.Security.Claims;
using Website_Progress.Helpers;
using Website_Progress.Interfaces;
using Website_Progress.Models;
using Website_Progress.ModelsDTO;
using Website_Progress.Services;

namespace Website_Progress.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly ICartRepository _cartRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly TelegramService _telegramService;


        public OrderController(
    ICartRepository cartRepository,
    IOrderRepository orderRepository,
    TelegramService telegramService)
        {
            _cartRepository = cartRepository;
            _orderRepository = orderRepository;
            _telegramService = telegramService;
        }
        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        public async Task<IActionResult> Index()
        {
            var cart = await _cartRepository.TryGetByUserIdAsync(GetUserId());

            var order = new OrderViewModel
            {
                Items = cart?.Items.ToCartItemViewModels()
            };

            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> Buy(OrderViewModel order)
        {
            var cart = await _cartRepository.TryGetByUserIdAsync(GetUserId());

            if (cart == null)
            {
                return View(nameof(Index), order);
            }

            order.Items = cart.Items.ToCartItemViewModels();
            order.UserId = GetUserId();

            if (!ModelState.IsValid)
            {
                return View(nameof(Index), order);
            }

            var orderDb = new Order()
            {
                UserId = order.UserId,
                Items = cart.Items,
                DeliveryUser = order.DeliveryUser.ToDeliveryUserDb()
            };

            await _orderRepository.AddAsync(orderDb);

            await _telegramService.SendOrderAsync(orderDb);
            await _cartRepository.ClearAsync(GetUserId());

            return RedirectToAction(nameof(Success), new { id = orderDb.Id });
        }

        public IActionResult Success(Guid id)
        {
            ViewBag.OrderId = id;
            return View();
        }

        public async Task<IActionResult> GenerateInvoice(Guid id)
        {
            var order = await _orderRepository.TryGetByIdAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            var document = new InvoiceDocument(order);

            var pdf = document.GeneratePdf();

            return File(pdf, "application/pdf", $"Invoice_{order.Id}.pdf");
        }
    }

}
