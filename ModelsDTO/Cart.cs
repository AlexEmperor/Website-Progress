namespace Website_Progress.ModelsDTO
{
    public class Cart
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public List<CartItem> Items { get; set; }
        public DateTime CreationDateTime { get; set; }

    }
}
