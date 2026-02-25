using Microsoft.AspNetCore.Mvc;
using Website_Progress.Helpers;
using Website_Progress.Interfaces;

namespace Website_Progress.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsRepository _newsRepository;

        public NewsController(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public IActionResult Index(int id)
        {
            var New = _newsRepository.TryGetById(id);
            return View(New.ToNewsViewModel());

        }

        public IActionResult All()
        {
            var products = _newsRepository.GetAll();
            return View(products.ToNewsViewModels());
        }
    }
}
