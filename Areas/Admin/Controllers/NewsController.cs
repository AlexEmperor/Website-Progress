using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Website_Progress.Helpers;
using Website_Progress.Interfaces;
using Website_Progress.Models;

namespace Website_Progress.Areas.Admin.Controllers
{
    [Area(Constants.AdminRoleName)]
    [Authorize(Roles = Constants.AdminRoleName)]
    public class NewsController : Controller
    {
        private readonly INewsRepository _newsRepository;
        private readonly IWebHostEnvironment _environment;

        public NewsController(INewsRepository newsRepository, IWebHostEnvironment environment)
        {
            _newsRepository = newsRepository;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            var news = await _newsRepository.GetAllAsync();

            return View(news
                .ToNewsViewModels()
                .OrderByDescending(x => x.Date)
                .ToList());
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(NewsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Фото ОБЯЗАТЕЛЬНО при создании
            if (model.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Необходимо загрузить фото товара");
                return View(model);
            }

            model.ImagePath = await FileSaver.SaveFileAsync(
                model.ImageFile,
                "img",
                _environment,
                model.Title);

            await _newsRepository.AddAsync(model.ToNewsDb());

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _newsRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Update(int id)
        {
            var existingNews = await _newsRepository.TryGetByIdAsync(id);

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
        public async Task<IActionResult> Update(EditNewsViewModel model)
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
            newsDb.IsOnMainPage = model.IsOnMainPage;

            // ===== Фото =====
            if (model.ImageFile != null)
            {
                // Удаляем старый файл
                if (!string.IsNullOrEmpty(newsDb.ImageUrl))
                {
                    var oldPhotoPath = Path.Combine(_environment.WebRootPath,
                        newsDb.ImageUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

                    if (System.IO.File.Exists(oldPhotoPath))
                    {
                        System.IO.File.Delete(oldPhotoPath);
                    }
                }

                var newPhoto = await FileSaver.SaveFileAsync(model.ImageFile, "img", _environment, model.Title);
                if (newPhoto != null)
                {
                    newsDb.ImageUrl = newPhoto;
                }
            }

            await _newsRepository.UpdateAsync(newsDb);

            return RedirectToAction(nameof(Index));
        }
    }
}
