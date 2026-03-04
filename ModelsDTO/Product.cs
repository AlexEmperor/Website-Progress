namespace Website_Progress.ModelsDTO
{
    public class Product
    {
        public int Id { get; set; }


        public string Name { get; set; }


        public decimal Cost { get; set; }

        public string? Description { get; set; }
        public string? PhotoPath { get; set; }
        public string? PresentationPath { get; set; }

        public bool IsOnMainPage { get; set; }
        public string? FirmwarePath { get; set; }
    }
}
