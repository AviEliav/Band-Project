using Microsoft.AspNetCore.Mvc;

namespace BandProject.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
