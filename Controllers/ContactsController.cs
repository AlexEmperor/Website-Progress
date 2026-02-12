using Microsoft.AspNetCore.Mvc;

namespace WEBtest.Controllers
{
    public class ContactsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
