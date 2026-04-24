namespace Website_Progress.Controllers
{
    public class HomeController(
        IProductRepository productRepository,
        INewsRepository newsRepository) : Controller
    {
        private readonly IProductRepository _productRepository = productRepository;
        private readonly INewsRepository _newsRepository = newsRepository;

        public async Task<IActionResult> Index()
        {
            var productsTask = await _productRepository.GetForMainPageAsync();
            var newsTask = await _newsRepository.GetForMainPageAsync();

            var model = new HomeViewModel
            {
                News = newsTask.ToNewsViewModels(),
                FeaturedProducts = productsTask.ToProductViewModels()
            };

            return View(model);
        }

        public async Task<IActionResult> Search(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return View();
            }

            var products = await _productRepository.SearchAsync(query);

            return View(products.ToProductViewModels());
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
