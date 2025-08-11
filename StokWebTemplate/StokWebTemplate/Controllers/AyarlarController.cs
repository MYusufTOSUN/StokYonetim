using Microsoft.AspNetCore.Mvc;

namespace StokWeb.Controllers
{
    public class AyarlarController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Kullanici = HttpContext.Session.GetString("KULLANICI_ADI");
            return View();
        }
    }
}
