using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class Usuario : Controller
    {
        public IActionResult Index()
        {

            return View();
        }
    }
}
