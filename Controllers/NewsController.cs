using Microsoft.AspNetCore.Mvc;
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
            return View(New);
        }

        public IActionResult All()
        {
            return View(_newsRepository.GetAll());
        }
    }
}
