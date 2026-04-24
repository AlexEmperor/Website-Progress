namespace Website_Progress.Controllers
{
    public class FooterPagesController : Controller
    {
        [Route("support")]
        public IActionResult Support()
        {
            return View();
        }

        [Route("service")]
        public IActionResult Service()
        {
            return View();
        }

        [Route("delivery")]
        public IActionResult Delivery()
        {
            return View();
        }

        [Route("services")]
        public IActionResult Services()
        {
            return View();
        }
    }
}
