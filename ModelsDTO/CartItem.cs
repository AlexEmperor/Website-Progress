namespace Website_Progress.ModelsDTO
{
    public class CartItem
    {
        public Guid Id { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public Cart? Cart { get; set; }

        // Цена фиксируется в момент добавления в корзину
        public decimal PriceAtPurchase { get; set; }
    }

}
