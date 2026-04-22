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
        private readonly IFileStorage _storage;

        public ProductController(IProductRepository productsRepository, IFileStorage storage)
        {
            _productsRepository = productsRepository;
            _storage = storage;
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

            if (model.PhotoFiles == null || !model.PhotoFiles.Any(f => f != null && f.Length > 0))
            {
                ModelState.AddModelError("PhotoFiles", "Необходимо загрузить хотя бы одно фото товара");
                return View(model);
            }

            var savedPaths = new List<string>();
            foreach (var file in model.PhotoFiles.Where(f => f != null && f.Length > 0))
            {
                var url = await _storage.SaveAsync(file, "products", model.Name);
                if (!string.IsNullOrEmpty(url))
                {
                    savedPaths.Add(url);
                }
            }
            model.PhotoPath = string.Join(";", savedPaths);

            model.PresentationPath = await _storage.SaveAsync(model.PresentationFile, "presentations", model.Name);
            model.FirmwarePath = await _storage.SaveAsync(model.FirmwareFile, "firmware", model.Name);

            await _productsRepository.AddAsync(model.ToProductDb());
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productsRepository.TryGetByIdAsync(id);
            if (product != null)
            {
                foreach (var url in (product.PhotoPath ?? "").Split(';', StringSplitOptions.RemoveEmptyEntries))
                {
                    await _storage.DeleteAsync(url.Trim());
                }

                await _storage.DeleteAsync(product.PresentationPath);
                await _storage.DeleteAsync(product.FirmwarePath);
            }

            await _productsRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            var existing = await _productsRepository.TryGetByIdAsync(id);
            return View(existing?.ToProductViewModel());
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
            var currentPaths = (productDb.PhotoPath ?? "")
                .Split(';', StringSplitOptions.RemoveEmptyEntries)
                .Select(p => p.Trim())
                .Where(p => !string.IsNullOrEmpty(p))
                .ToList();

            if (photosToDelete != null)
            {
                foreach (var toDelete in photosToDelete)
                {
                    if (currentPaths.Remove(toDelete))
                    {
                        await _storage.DeleteAsync(toDelete);
                    }
                }
            }

            if (model.PhotoFiles != null)
            {
                foreach (var file in model.PhotoFiles.Where(f => f != null && f.Length > 0))
                {
                    var url = await _storage.SaveAsync(file, "products", model.Name);
                    if (!string.IsNullOrEmpty(url))
                    {
                        currentPaths.Add(url);
                    }
                }
            }

            productDb.PhotoPath = currentPaths.Any() ? string.Join(";", currentPaths) : null;

            // ===== Презентация =====
            if (model.PresentationFile != null)
            {
                await _storage.DeleteAsync(productDb.PresentationPath);
                var url = await _storage.SaveAsync(model.PresentationFile, "presentations", model.Name);
                if (url != null)
                {
                    productDb.PresentationPath = url;
                }
            }

            // ===== Прошивка =====
            if (model.FirmwareFile != null)
            {
                await _storage.DeleteAsync(productDb.FirmwarePath);
                var url = await _storage.SaveAsync(model.FirmwareFile, "firmware", model.Name);
                if (url != null)
                {
                    productDb.FirmwarePath = url;
                }
            }

            await _productsRepository.UpdateAsync(productDb);
            return RedirectToAction(nameof(Index));
        }
    }
}