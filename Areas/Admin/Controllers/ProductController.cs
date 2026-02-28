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


        public IActionResult Index()
        {
            var products = _productsRepository.GetAll().OrderBy(p => p.Id).ToList();
            return View(products.ToProductViewModels());
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

            model.PhotoPath = await FileSaver.SaveFileAsync(model.PhotoFile, "img", _environment);
            model.PresentationPath = await FileSaver.SaveFileAsync(model.PresentationFile, "presentations", _environment);
            model.FirmwarePath = await FileSaver.SaveFileAsync(model.FirmwareFile, "firmware", _environment);

            _productsRepository.Add(model.ToProductDb());

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            _productsRepository.Delete(id);

            return RedirectToAction(nameof(Index));
        }


        public IActionResult Update(int id)
        {
            var existingProduct = _productsRepository.TryGetById(id);
            return View(existingProduct?.ToProductViewModel());
            //return View(existingProduct);
        }


        [HttpPost]
        public async Task<IActionResult> Update(ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var productDb = _productsRepository.TryGetById(model.Id);
            if (productDb == null)
            {
                return NotFound();
            }

            productDb.Name = model.Name;
            productDb.Cost = model.Cost;
            productDb.Description = model.Description;

            // Если загрузили новый файл — заменяем
            var newPhoto = await FileSaver.SaveFileAsync(model.PhotoFile, "img", _environment);
            if (newPhoto != null)
            {
                productDb.PhotoPath = newPhoto;
            }

            var newPresentation = await FileSaver.SaveFileAsync(model.PresentationFile, "presentations", _environment);
            if (newPresentation != null)
            {
                productDb.PresentationPath = newPresentation;
            }

            var newFirmware = await FileSaver.SaveFileAsync(model.FirmwareFile, "firmware", _environment);
            if (newFirmware != null)
            {
                productDb.FirmwarePath = newFirmware;
            }

            _productsRepository.Update(productDb);

            return RedirectToAction(nameof(Index));
        }
    }
}
