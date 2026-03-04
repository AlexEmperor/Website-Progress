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

        public IActionResult Index()
        {
            return View(_newsRepository.GetAll().ToNewsViewModels().OrderByDescending(x => x.Date).ToList());
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

            _newsRepository.Add(model.ToNewsDb());

            return RedirectToAction(nameof(Index));
        }


        public IActionResult Delete(int id)
        {
            _newsRepository.Delete(id);

            return RedirectToAction(nameof(Index));
        }


        public IActionResult Update(int id)
        {
            var existingNews = _newsRepository.TryGetById(id);

            var model = new EditNewsViewModel
            {
                Id = existingNews.Id,
                Title = existingNews.Title,
                Description = existingNews.ShortText,
                ImagePath = existingNews.ImageUrl
            };

            return View(model);
            //return View(existingProduct);
        }


        [HttpPost]
        public async Task<IActionResult> Update(EditNewsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var newsDb = _newsRepository.TryGetById(model.Id);
            if (newsDb == null)
            {
                return NotFound();
            }

            newsDb.Title = model.Title;
            newsDb.ShortText = model.Description;
            //newsDb.ImageUrl = model.ImagePath;

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

            _newsRepository.Update(newsDb);

            return RedirectToAction(nameof(Index));
        }
    }
}
