using Microsoft.AspNetCore.Mvc;
using ProyectoWebG2.Models;

namespace ProyectoWebG2.Controllers
{
    public class CursosController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public CursosController(IHttpClientFactory factory, IConfiguration configuration)
        {
            _httpClient = factory.CreateClient("api");
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var url = _configuration["Valores:UrlAPI"] + "Cursos";
                var cursos = await _httpClient.GetFromJsonAsync<List<Curso>>(url);
                return View(cursos ?? new List<Curso>());
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al cargar cursos: {ex.Message}";
                return View(new List<Curso>());
            }
        }
    }
}
