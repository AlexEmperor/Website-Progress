using Microsoft.AspNetCore.Mvc;
using Website_Progress.Interfaces;
using Website_Progress.Models;

namespace Website_Progress.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _usersRepository;
        //private readonly IRoleRepository _rolesRepository;


        public AccountController(IUserRepository usersRepository/*, IRoleRepository rolesRepository*/)
        {
            //_rolesRepository = rolesRepository;
            _usersRepository = usersRepository;
        }
        public IActionResult Autorization()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Autorization(Autorization autorization)
        {
            if (autorization.Password == autorization.Login)
            {
                ModelState.AddModelError("", "Логин и пароль не должны совпадать");
            }

            var existingUser = _usersRepository.TryGetByLogin(autorization.Login);

            if (existingUser == null)
            {
                ModelState.AddModelError("", "Такого пользователя не существует!\r\nПройдите регистрацию!");
            }

            if (autorization.Password != existingUser?.Password)
            {
                ModelState.AddModelError("", "Неправильный пароль пользователя!");
            }
            return !ModelState.IsValid ? View(autorization) : RedirectToAction(nameof(Index), "Home");
        }

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registration(Registration registration)
        {
            if (registration.Password == registration.Login)
            {
                ModelState.AddModelError("", "Логин и пароль не должны совпадать");
            }
            var existingUser = _usersRepository.TryGetByLogin(registration.Login);

            if (existingUser != null)
            {
                ModelState.AddModelError("", "Пользователь с таким логином уже зарегистрирован!\r\n" +
                    "Необходимо зарегистрироваться под другим логином!");
            }

            if (!ModelState.IsValid)
            {
                return View(registration);
            }

            var user = new User()
            {
                Login = registration.Login,
                Password = registration.Password,
                FirstName = registration.FirstName,
                LastName = registration.LastName,
                Phone = registration.Phone,
            };

            _usersRepository.Add(user);

            return RedirectToAction(nameof(Index), "Home");

            //return View();
        }
    }
}
