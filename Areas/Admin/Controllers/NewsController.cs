namespace Website_Progress.Areas.Admin.Controllers
{
    [Area(Constants.AdminRoleName)]
    [Authorize(Roles = Constants.AdminRoleName)]
    public class NewsController : Controller
    {
        private readonly INewsRepository _newsRepository;
        private readonly IFileStorage _storage;

        public NewsController(INewsRepository newsRepository, IFileStorage storage)
        {
            _newsRepository = newsRepository;
            _storage = storage;
        }

        public async Task<IActionResult> Index()
        {
            var news = await _newsRepository.GetAllAsync();

            return View(news
                .ToNewsViewModels()
                .OrderByDescending(x => x.Date)
                .ToList());
        }

        public IActionResult Add() => View();

        [HttpPost]
        public async Task<IActionResult> Add(NewsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Необходимо загрузить фото новости");
                return View(model);
            }

            model.ImagePath = await _storage.SaveAsync(model.ImageFile, "news", model.Title);

            await _newsRepository.AddAsync(model.ToNewsDb());
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var news = await _newsRepository.TryGetByIdAsync(id);
            if (news != null)
            {
                // Чистим файл из хранилища
                await _storage.DeleteAsync(news.ImageUrl);
            }

            await _newsRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            var existingNews = await _newsRepository.TryGetByIdAsync(id);
            if (existingNews == null)
            {
                return NotFound();
            }

            var model = new EditNewsViewModel
            {
                Id = existingNews.Id,
                Title = existingNews.Title,
                Description = existingNews.Description,
                ImagePath = existingNews.ImageUrl
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(EditNewsViewModel model, string? toggleMainPage)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var newsDb = await _newsRepository.TryGetByIdAsync(model.Id);
            if (newsDb == null)
            {
                return NotFound();
            }

            newsDb.Title = model.Title;
            newsDb.Description = model.Description;

            if (!string.IsNullOrEmpty(toggleMainPage))
            {
                newsDb.IsOnMainPage = !newsDb.IsOnMainPage;
            }

            // ===== Фото =====
            if (model.ImageFile != null)
            {
                // Удаляем старое фото из хранилища
                await _storage.DeleteAsync(newsDb.ImageUrl);

                var newUrl = await _storage.SaveAsync(model.ImageFile, "news", model.Title);
                if (newUrl != null)
                {
                    newsDb.ImageUrl = newUrl;
                }
            }

            await _newsRepository.UpdateAsync(newsDb);
            return RedirectToAction(nameof(Index));
        }
    }
}