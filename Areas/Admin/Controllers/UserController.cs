using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Website_Progress.Models;
using Website_Progress.ModelsDTO;

namespace Website_Progress.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Constants.AdminRoleName)]
    public class UserController : Controller
    {
        private readonly UserManager<UserDTO> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(
            UserManager<UserDTO> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList();
            var model = new List<AdminUserViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user); // возвращает IList<string>
                model.Add(new AdminUserViewModel
                {
                    Id = user.Id,
                    Email = user.Email!,
                    Phone = user.PhoneNumber,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Login = user.Email,
                    Role = roles.FirstOrDefault() ?? "User" // берём первую роль или дефолт
                });
            }

            return View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Detail(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var roles = await _userManager.GetRolesAsync(user);

            var model = new AdminUserViewModel
            {
                Id = user.Id,
                Login = user.Email,
                Email = user.Email!,
                Phone = user.PhoneNumber,
                FirstName = user.FirstName,
                CreationDateTime = user.CreationDateTime,
                LastName = user.LastName,
                Role = roles.FirstOrDefault() ?? "User"
            };

            return View(model);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AdminUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new UserDTO
            {
                UserName = model.Login,
                Email = model.Email,
                PhoneNumber = model.Phone,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }

            // Назначаем роль (по умолчанию "User")
            await _userManager.AddToRoleAsync(user, model.Role ?? Constants.UserRoleName);

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var roles = await _userManager.GetRolesAsync(user);

            var model = new AdminUserViewModel
            {
                Id = user.Id,
                Login = user.Email,
                Email = user.Email!,
                Phone = user.PhoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = roles.FirstOrDefault() ?? "User"
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(AdminUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return RedirectToAction(nameof(Index));
            }

            user.Email = model.Email;
            user.UserName = model.Login;
            user.PhoneNumber = model.Phone;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }

            // Обновление роли
            var currentRoles = await _userManager.GetRolesAsync(user);
            if (!string.IsNullOrEmpty(model.Role) && !currentRoles.Contains(model.Role))
            {
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                await _userManager.AddToRoleAsync(user, model.Role);
            }

            return RedirectToAction(nameof(Detail), new { id = user.Id });
        }


        public async Task<IActionResult> ChangeRole(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var roles = _roleManager.Roles.Select(r => r.Name).ToList();

            ViewBag.AllRoles = roles;
            ViewBag.UserRoles = userRoles;
            ViewBag.UserId = id;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangeRole(string id, string role)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            await _userManager.AddToRoleAsync(user, role);

            return RedirectToAction(nameof(Index));
        }
    }
}
