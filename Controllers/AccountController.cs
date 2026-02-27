using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Website_Progress.Models;
using Website_Progress.ModelsDTO;

namespace Website_Progress.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<UserDTO> _userManager;
        private readonly SignInManager<UserDTO> _signInManager;

        public AccountController(UserManager<UserDTO> userManager, SignInManager<UserDTO> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Autorization(string? returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public IActionResult Autorization(Autorization authorization, string? returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(authorization);
            }

            var result = _signInManager.PasswordSignInAsync(
                authorization.Login,
                authorization.Password,
                authorization.Memorize,
                false).Result;

            if (result.Succeeded)
            {
                return !string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)
                    ? Redirect(returnUrl)
                    : RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Неверный логин или пароль");
            return View(authorization);
        }

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registration(Registration registration)
        {
            if (!ModelState.IsValid)
            {
                return View(registration);
            }

            var user = new UserDTO
            {
                UserName = registration.Login,
                Email = registration.Login,
                PhoneNumber = registration.Phone,
                CreationDateTime = registration.CreationDateTime,
                FirstName = registration.FirstName,
                LastName = registration.LastName
            };

            var result = _userManager.CreateAsync(user, registration.Password).Result;

            if (result.Succeeded)
            {
                // Назначаем роль
                _userManager.AddToRoleAsync(user, Constants.UserRoleName).Wait();

                // Автоматический вход после регистрации
                _signInManager.SignInAsync(user, false).Wait();

                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(registration);
        }

        public IActionResult Logout()
        {
            _signInManager.SignOutAsync().Wait();
            return RedirectToAction("Index", "Home");
        }
    }
}
