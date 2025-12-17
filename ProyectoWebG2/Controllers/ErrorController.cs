using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace SM_ProyectoWeb.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult MostrarError()
        {
            var excepcion = HttpContext.Features.Get<IExceptionHandlerFeature>();

            return View();
        }
    }
}
