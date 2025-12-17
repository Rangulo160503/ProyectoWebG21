using Microsoft.AspNetCore.Mvc;
using ProyectoWebG2.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace ProyectoWebG2.Controllers
{
    public class AsignarCursosController : Controller
    {
        private readonly IHttpClientFactory _factory;
        public AsignarCursosController(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _factory.CreateClient("api");
            var cursosResponse = await client.GetAsync("api/Cursos");
            cursosResponse.EnsureSuccessStatusCode();
            var cursosJson = await cursosResponse.Content.ReadAsStringAsync();
            var cursos = JsonSerializer.Deserialize<List<Curso>>(cursosJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var instructoresResponse = await client.GetAsync("api/Instructores");
            instructoresResponse.EnsureSuccessStatusCode();
            var instructoresJson = await instructoresResponse.Content.ReadAsStringAsync();
            var instructores = JsonSerializer.Deserialize<List<InstructorVM>>(instructoresJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            ViewBag.Cursos = cursos ?? new List<Curso>();
            ViewBag.Instructores = instructores ?? new List<InstructorVM>();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(int idCurso, int idInstructor)
        {
            var client = _factory.CreateClient("api");
            var payload = new { IdCurso = idCurso, IdInstructor = idInstructor };
            var res = await client.PostAsJsonAsync("admin/asignar-curso", payload);
            if (res.IsSuccessStatusCode)
            {
                TempData["Msg"] = "Curso asignado correctamente";
            }
            else
            {
                TempData["Error"] = "No se pudo asignar el curso";
            }
            return RedirectToAction("Index");
        }
    }
}
