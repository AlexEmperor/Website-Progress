namespace Website_Progress.Areas.Admin.Controllers
{
    /* [Area(Constants.AdminRoleName)]
     [Authorize(Roles = Constants.AdminRoleName)]
     public class RoleController : Controller
     {
         private readonly IRoleRepository _rolesRepository;


         public RoleController(IRoleRepository rolesRepository)
         {
             _rolesRepository = rolesRepository;

         }


         public IActionResult Index()
         {
             var roles = _rolesRepository.GetAll();

             return View(roles);
         }


         public IActionResult Add()
         {
             return View();
         }


         [HttpPost]
         public IActionResult Add(Role role)
         {
             if (_rolesRepository.TryGetByName(role.Name) != null)
             {
                 ModelState.AddModelError("",
                     "Такая роль уже существует!");
             }

             if (!ModelState.IsValid)
             {
                 return View(role);
             }

             _rolesRepository.Add(role);

             return RedirectToAction(nameof(Index));
         }


         public IActionResult Delete(Guid roleId)
         {
             _rolesRepository.Delete(roleId);

             return RedirectToAction(nameof(Index));
         }
     }*/
}
