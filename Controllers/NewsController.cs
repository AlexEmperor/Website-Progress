using Microsoft.AspNetCore.Mvc;
using Website_Progress.Helpers;

namespace Website_Progress.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsRepository _newsRepository;

        public NewsController(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public async Task<IActionResult> Index(int id)
        {
            var news = await _newsRepository.TryGetByIdAsync(id);

            return news == null ? NotFound() : View(news.ToNewsViewModel());
        }

        public async Task<IActionResult> All()
        {
            var news = await _newsRepository.GetAllAsync();
            return View(news.ToNewsViewModels().OrderByDescending(x => x.Date).ToList());
        }
    }
}
