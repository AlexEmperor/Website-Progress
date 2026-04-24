namespace Website_Progress.Controllers
{
    public class CatalogController(IProductRepository productRepository) : Controller
    {
        private readonly IProductRepository _productRepository = productRepository;

        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllAsync();

            var ordered = products
                .OrderBy(p => p.Id)
                .ToList();

            return View(ordered.ToProductViewModels());
        }
    }
}
