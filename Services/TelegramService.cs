namespace Website_Progress.Services
{
    public class TelegramService(TelegramBotClient bot)
    {
        private readonly TelegramBotClient _bot = bot;

        public async Task SendOrderAsync(Order order)
        {
            var adminChatId = -5123579887;
            var totalCost = order.Items.Sum(i => i.Product.Cost * i.Quantity);

            var itemsText = string.Join("\n",
                order.Items.Select(x =>
                    $"• {x.Product.Name} — {x.Quantity} шт. ({x.Product.Cost:c}) за 1 товар"));

            var text =
                $"📦 <b>Новый заказ</b>\n\n" +
                $"👤 Имя: {order.DeliveryUser.Name}\n\n" +
                $"🛒 <b>Состав заказа:</b>\n{itemsText}\n\n" +
                $"💬 Комментарий: {order.DeliveryUser.Comment}\n\n" +
                $"💬 Предполагаемая дата доставки: {order.DeliveryUser.Date}\n\n" +
                $"📧 Email: {order.DeliveryUser.Email}\n" +
                $"📱 Телефон: {order.DeliveryUser.Phone}\n" +
                $"📱 Telegram: {order.DeliveryUser.Telegram}\n\n" +
                $"💰 <b>Сумма: {totalCost:c}</b>";

            await _bot.SendMessage(
                adminChatId,
                text,
                parseMode: ParseMode.Html
            );
        }
    }
}
