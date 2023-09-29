using Microsoft.AspNetCore.Mvc;

namespace Lab.Areas.Admins.Controllers
{
    [Area("Admins")]
    public class HomeController : Controller
    {
        //[Area("Admins")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
