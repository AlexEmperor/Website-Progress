namespace Website_Progress.Areas.Admin.Controllers
{
    [Area(Constants.AdminRoleName)]
    [Authorize(Roles = Constants.AdminRoleName)]
    public class SettingsController : Controller
    {
        private readonly DatabaseContext _db;
        private readonly IMemoryCache _cache;

        public SettingsController(DatabaseContext db, IMemoryCache cache)
        {
            _db = db;
            _cache = cache;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SetMode(SiteMode mode)
        {
            var settings = await _db.SiteSettings.FirstOrDefaultAsync();

            if (settings == null)
            {
                settings = new SiteSetting
                {
                    Mode = mode
                };
                _db.SiteSettings.Add(settings);
            }
            else
            {
                settings.Mode = mode;
            }
            await _db.SaveChangesAsync();

            _cache.Remove("SiteSettings");

            return RedirectToAction("Index");
        }
    }

}
