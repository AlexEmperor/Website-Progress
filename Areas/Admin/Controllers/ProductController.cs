using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Website_Progress.Helpers;
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
            return View(products.OrderBy(p => p.Id).ToList().ToProductViewModels());
        }

        public IActionResult Add() => View();

        [HttpPost]
        public async Task<IActionResult> Add(ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Фото ОБЯЗАТЕЛЬНО при создании (хотя бы одно)
            if (model.PhotoFiles == null || !model.PhotoFiles.Any(f => f != null && f.Length > 0))
            {
                ModelState.AddModelError("PhotoFiles", "Необходимо загрузить хотя бы одно фото товара");
                return View(model);
            }

            // Сохраняем все загруженные фото, собираем пути через ';'
            var savedPaths = new List<string>();
            foreach (var file in model.PhotoFiles.Where(f => f != null && f.Length > 0))
            {
                var savedPath = await FileSaver.SaveFileAsync(file, "img", _environment, model.Name);
                if (!string.IsNullOrEmpty(savedPath))
                {
                    savedPaths.Add(savedPath);
                }
            }
            model.PhotoPath = string.Join(";", savedPaths);

            model.PresentationPath = await FileSaver.SaveFileAsync(
                model.PresentationFile, "presentations", _environment, model.Name);

            model.FirmwarePath = await FileSaver.SaveFileAsync(
                model.FirmwareFile, "firmware", _environment, model.Name);

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
        public async Task<IActionResult> Update(
            ProductViewModel model,
            string? toggleMainPage,
            List<string>? photosToDelete)
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
            productDb.ShortDescription = model.ShortDescription;

            if (!string.IsNullOrEmpty(toggleMainPage))
            {
                productDb.IsOnMainPage = !productDb.IsOnMainPage;
            }

            // ===== ФОТО =====
            var currentPaths = (productDb.PhotoPath ?? string.Empty)
                .Split(';', StringSplitOptions.RemoveEmptyEntries)
                .Select(p => p.Trim())
                .Where(p => !string.IsNullOrEmpty(p))
                .ToList();

            // 1) Удаляем отмеченные чекбоксами фото
            if (photosToDelete != null && photosToDelete.Any())
            {
                foreach (var toDelete in photosToDelete)
                {
                    if (currentPaths.Remove(toDelete))
                    {
                        var physical = Path.Combine(
                            _environment.WebRootPath,
                            toDelete.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                        if (System.IO.File.Exists(physical))
                        {
                            System.IO.File.Delete(physical);
                        }
                    }
                }
            }

            // 2) Добавляем новые загруженные фото в конец списка
            if (model.PhotoFiles != null)
            {
                foreach (var file in model.PhotoFiles.Where(f => f != null && f.Length > 0))
                {
                    var saved = await FileSaver.SaveFileAsync(file, "img", _environment, model.Name);
                    if (!string.IsNullOrEmpty(saved))
                    {
                        currentPaths.Add(saved);
                    }
                }
            }

            productDb.PhotoPath = currentPaths.Any()
                ? string.Join(";", currentPaths)
                : null;

            // ===== Презентация =====
            if (model.PresentationFile != null)
            {
                if (!string.IsNullOrEmpty(productDb.PresentationPath))
                {
                    var oldPath = Path.Combine(_environment.WebRootPath,
                        productDb.PresentationPath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }

                var newPresentation = await FileSaver.SaveFileAsync(
                    model.PresentationFile, "presentations", _environment, model.Name);
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
                    var oldPath = Path.Combine(_environment.WebRootPath,
                        productDb.FirmwarePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }

                var newFirmware = await FileSaver.SaveFileAsync(
                    model.FirmwareFile, "firmware", _environment, model.Name);
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