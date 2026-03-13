using Microsoft.AspNetCore.Mvc;

namespace Website_Progress.Controllers
{
    public class MaintenanceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
