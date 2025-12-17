using Microsoft.AspNetCore.Mvc;

namespace ProyectoWebG2.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
