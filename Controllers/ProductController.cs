namespace Website_Progress.Controllers
{
    public class ProductController(IProductRepository productRepository) : Controller
    {
        private readonly IProductRepository _productRepository = productRepository;

        public async Task<IActionResult> Index(int id)
        {
            var product = await _productRepository.TryGetByIdAsync(id);
            if (product == null)
            {
                return View((ProductViewModel?)null);
            }

            var vm = product.ToProductViewModel();

            var content = ProductDescriptionParser.Parse(vm.Description);
            ViewData["Content"] = content;

            return View(vm);
        }
    }
}

