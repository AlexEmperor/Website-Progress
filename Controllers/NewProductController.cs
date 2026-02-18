using Microsoft.AspNetCore.Mvc;
using Website_Progress.Interfaces;

namespace Website_Progress.Controllers
{
    public class NewProductController : Controller
    {
        private readonly INewProductRepository _newProductRepository;

        public NewProductController(INewProductRepository newProductRepository)
        {
            _newProductRepository = newProductRepository;
        }

        public IActionResult Index(int id)
        {
            var product = _newProductRepository.TryGetById(id);
            //return View(product?.ToProductViewModel());
            return View(product);
        }
    }
}
