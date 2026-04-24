namespace Website_Progress.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Наименование товара", Prompt = "Наименование товара")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Display(Name = "Цена, руб.", Prompt = "Цена, руб.")]
        public decimal Cost { get; set; }

        [Display(Name = "Описание товара", Prompt = "Описание товара")]
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }

        [Display(Name = "Краткое описание товара", Prompt = "Краткое описание товара")]
        [DataType(DataType.MultilineText)]
        public string? ShortDescription { get; set; }

        public string? PhotoPath { get; set; }

        [Display(Name = "Путь до презентации", Prompt = "/wwwroot/presentations/product.pptx")]
        [DataType(DataType.Text)]
        public string? PresentationPath { get; set; }

        [Display(Name = "Путь до прошивки", Prompt = "/wwwroot/firmware/product")]
        [DataType(DataType.Text)]
        public string? FirmwarePath { get; set; }
        public bool IsOnMainPage { get; set; }
        public IFormFile? PhotoFile { get; set; }
        public List<IFormFile>? PhotoFiles { get; set; }
        public List<string> PhotoPaths =>
    string.IsNullOrWhiteSpace(PhotoPath)
        ? []
        : PhotoPath
            .Split(';', StringSplitOptions.RemoveEmptyEntries)
            .Select(p => p.Trim())
            .Where(p => !string.IsNullOrEmpty(p))
            .ToList();

        public IFormFile? PresentationFile { get; set; }
        public IFormFile? FirmwareFile { get; set; }
        public string? CoverPhoto => PhotoPaths.FirstOrDefault();

        [Display(Name = "Статус")]
        public ProductStatusViewModel Status { get; set; } = ProductStatusViewModel.Development;
        public ProductViewModel() { }
    }
}
