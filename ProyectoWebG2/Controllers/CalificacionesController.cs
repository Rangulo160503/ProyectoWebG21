using Microsoft.AspNetCore.Mvc;
using ProyectoWebG2.Models;
using System.Text.Json;

namespace ProyectoWebG2.Controllers
{
    public class CalificacionesController : Controller
    {
        private readonly IHttpClientFactory _factory;
        public CalificacionesController(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _factory.CreateClient("api");
            var response = await client.GetAsync("admin/calificaciones");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<List<CalificacionVM>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return View(data ?? new List<CalificacionVM>());
        }
    }
}
