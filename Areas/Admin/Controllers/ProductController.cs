using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Website_Progress.Helpers;
using Website_Progress.Interfaces;
using Website_Progress.Models;

namespace Website_Progress.Areas.Admin.Controllers
{
    [Area(Constants.AdminRoleName)]
    [Authorize(Roles = Constants.AdminRoleName)]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productsRepository;
        private readonly IWebHostEnvironment _environment;


        public ProductController(IProductRepository productsRepository, IWebHostEnvironment environment)
        {
            _productsRepository = productsRepository;
            _environment = environment;
        }


        public async Task<IActionResult> Index()
        {
            var products = await _productsRepository.GetAllAsync();

            return View(products
                .OrderBy(p => p.Id)
                .ToList()
                .ToProductViewModels());
        }


        public IActionResult Add()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Add(ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Фото ОБЯЗАТЕЛЬНО при создании
            if (model.PhotoFile == null)
            {
                ModelState.AddModelError("PhotoFile", "Необходимо загрузить фото товара");
                return View(model);
            }

            model.PhotoPath = await FileSaver.SaveFileAsync(
                model.PhotoFile,
                "img",
                _environment,
                model.Name);

            model.PresentationPath = await FileSaver.SaveFileAsync(
                model.PresentationFile,
                "presentations",
                _environment,
                model.Name);

            model.FirmwarePath = await FileSaver.SaveFileAsync(
                model.FirmwareFile,
                "firmware",
                _environment,
                model.Name);

            await _productsRepository.AddAsync(model.ToProductDb());

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _productsRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            var existingProduct = await _productsRepository.TryGetByIdAsync(id);
            return View(existingProduct?.ToProductViewModel());
        }


        [HttpPost]
        public async Task<IActionResult> Update(ProductViewModel model, string? toggleMainPage)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var productDb = await _productsRepository.TryGetByIdAsync(model.Id);
            if (productDb == null)
            {
                return NotFound();
            }

            productDb.Name = model.Name;
            productDb.Cost = model.Cost;
            productDb.Description = model.Description;
            // ===== Переключение главной кнопкой =====
            if (!string.IsNullOrEmpty(toggleMainPage))
            {
                productDb.IsOnMainPage = !productDb.IsOnMainPage;
            }

            // ===== Фото =====
            if (model.PhotoFile != null)
            {
                // Удаляем старый файл
                if (!string.IsNullOrEmpty(productDb.PhotoPath))
                {
                    var oldPhotoPath = Path.Combine(_environment.WebRootPath, productDb.PhotoPath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                    if (System.IO.File.Exists(oldPhotoPath))
                    {
                        System.IO.File.Delete(oldPhotoPath);
                    }
                }

                var newPhoto = await FileSaver.SaveFileAsync(model.PhotoFile, "img", _environment, model.Name);
                if (newPhoto != null)
                {
                    productDb.PhotoPath = newPhoto;
                }
            }

            // ===== Презентация =====
            if (model.PresentationFile != null)
            {
                if (!string.IsNullOrEmpty(productDb.PresentationPath))
                {
                    var oldPresentationPath = Path.Combine(_environment.WebRootPath, productDb.PresentationPath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                    if (System.IO.File.Exists(oldPresentationPath))
                    {
                        System.IO.File.Delete(oldPresentationPath);
                    }
                }

                var newPresentation = await FileSaver.SaveFileAsync(model.PresentationFile, "presentations", _environment, model.Name);
                if (newPresentation != null)
                {
                    productDb.PresentationPath = newPresentation;
                }
            }

            // ===== Прошивка =====
            if (model.FirmwareFile != null)
            {
                if (!string.IsNullOrEmpty(productDb.FirmwarePath))
                {
                    var oldFirmwarePath = Path.Combine(_environment.WebRootPath, productDb.FirmwarePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                    if (System.IO.File.Exists(oldFirmwarePath))
                    {
                        System.IO.File.Delete(oldFirmwarePath);
                    }
                }

                var newFirmware = await FileSaver.SaveFileAsync(model.FirmwareFile, "firmware", _environment, model.Name);
                if (newFirmware != null)
                {
                    productDb.FirmwarePath = newFirmware;
                }
            }

            await _productsRepository.UpdateAsync(productDb);
            return RedirectToAction(nameof(Index));
        }
    }
}
