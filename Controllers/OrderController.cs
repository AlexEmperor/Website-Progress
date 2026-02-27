using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public IActionResult Index()
        {
            var cart = _cartRepository.TryGetByUserId(GetUserId());

            var order = new OrderViewModel()
            {
                Items = cart?.Items.ToCartItemViewModels()
            };

            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> Buy(OrderViewModel order)
        {
            var cart = _cartRepository.TryGetByUserId(GetUserId());

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
                Id = order.Id,
                UserId = order.UserId,
                Items = cart.Items,
                DeliveryUser = order.DeliveryUser.ToDeliveryUserDb(),
                CreationDateTime = order.CreationDateTime,
                Status = (OrderStatus)order.Status
            };

            _orderRepository.Add(orderDb);

            await _telegramService.SendOrderAsync(orderDb);
            _cartRepository.Clear(GetUserId());

            return RedirectToAction(nameof(Success));
        }

        public IActionResult Success()
        {

            return View();
        }
    }

}
