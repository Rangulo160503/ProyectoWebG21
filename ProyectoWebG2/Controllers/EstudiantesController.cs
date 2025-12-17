using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoWebG2.Models;
using System.Net.Http.Json;

namespace ProyectoWebG2.Controllers
{
    public class EstudiantesController : Controller
    {
        private readonly IHttpClientFactory _factory;
        private readonly IConfiguration _configuration;

        public EstudiantesController(IHttpClientFactory factory, IConfiguration configuration)
        {
            _factory = factory;
            _configuration = configuration;
        }

        // GET: Estudiantes/Index - Panel principal
        [Seguridad]
        public IActionResult Index()
        {
            var nombreUsuario = HttpContext.Session.GetString("NombreUsuario") ?? "Estudiante";
            ViewBag.NombreUsuario = nombreUsuario;
            return View();
        }

        // GET: Estudiantes/CursosDisponibles - Ver cursos disponibles
        [Seguridad]
        public async Task<IActionResult> CursosDisponibles()
        {
            try
            {
                using var http = _factory.CreateClient();
                var url = _configuration["Valores:UrlAPI"] + "Cursos/Disponibles";
                var response = await http.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var cursos = await response.Content.ReadFromJsonAsync<List<CursoDisponibleVM>>();
                    return View(cursos ?? new List<CursoDisponibleVM>());
                }

                TempData["Error"] = "No se pudieron cargar los cursos disponibles.";
                return View(new List<CursoDisponibleVM>());
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al cargar cursos: {ex.Message}";
                return View(new List<CursoDisponibleVM>());
            }
        }

        // GET: Estudiantes/ObtenerHorarios - Obtener horarios de un curso (AJAX)
        [Seguridad]
        public async Task<IActionResult> ObtenerHorarios(int idCurso)
        {
            try
            {
                using var http = _factory.CreateClient();
                var url = _configuration["Valores:UrlAPI"] + $"Cursos/Horarios/{idCurso}";
                var response = await http.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var horarios = await response.Content.ReadFromJsonAsync<List<HorarioVM>>();
                    return Json(horarios ?? new List<HorarioVM>());
                }

                return Json(new List<HorarioVM>());
            }
            catch
            {
                return Json(new List<HorarioVM>());
            }
        }

        // POST: Estudiantes/Matricular - Matricular en un curso
        [Seguridad]
        [HttpPost]
        public async Task<IActionResult> Matricular([FromBody] MatricularRequestVM model)
        {
            int? idUsuario = HttpContext.Session.GetInt32("ConsecutivoUsuario");

            if (idUsuario == null || idUsuario == 0)
                return Json(new { success = false, message = "Sesión inválida. Por favor inicie sesión nuevamente." });

            try
            {
                using var http = _factory.CreateClient();
                var url = _configuration["Valores:UrlAPI"] + "Estudiantes/Matricular";

                // Armar objeto con el IdUsuario de la sesión
                var requestData = new
                {
                    IdUsuario = idUsuario,
                    IdCurso = model.IdCurso,
                    IdHorario = model.IdHorario
                };

                var response = await http.PostAsJsonAsync(url, requestData);
                var result = await response.Content.ReadFromJsonAsync<dynamic>();

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al conectar con la API: " + ex.Message });
            }
        }


        // GET: Estudiantes/CursosMatriculados - Ver cursos matriculados
        [Seguridad]
        public async Task<IActionResult> CursosMatriculados()
        {
            try
            {
                int? idUsuario = HttpContext.Session.GetInt32("ConsecutivoUsuario");
                if (idUsuario == null)
                {
                    return RedirectToAction("Login", "Home");
                }

                using var http = _factory.CreateClient();
                var url = _configuration["Valores:UrlAPI"] + $"Estudiantes/CursosMatriculados/{idUsuario}";
                var response = await http.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var cursos = await response.Content.ReadFromJsonAsync<List<CursoMatriculadoVM>>();
                    return View(cursos ?? new List<CursoMatriculadoVM>());
                }

                TempData["Error"] = "No se pudieron cargar los cursos matriculados.";
                return View(new List<CursoMatriculadoVM>());
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al cargar cursos: {ex.Message}";
                return View(new List<CursoMatriculadoVM>());
            }
        }

        // POST: Estudiantes/Renunciar - Renunciar a un curso
        [HttpPost]
        [Seguridad]
        public async Task<IActionResult> Renunciar(int idCurso)
        {
            try
            {
                int? idUsuario = HttpContext.Session.GetInt32("ConsecutivoUsuario");
                if (idUsuario == null)
                {
                    return Json(new { success = false, message = "Sesión no válida" });
                }

                using var http = _factory.CreateClient();
                var url = _configuration["Valores:UrlAPI"] + $"Estudiantes/Desmatricular/{idUsuario}/{idCurso}";
                var response = await http.PostAsync(url, null);

                if (response.IsSuccessStatusCode)
                {
                    var resultado = await response.Content.ReadFromJsonAsync<int>();
                    
                    if (resultado == 1)
                    {
                        return Json(new { success = true, message = "Has renunciado al curso exitosamente." });
                    }
                    else if (resultado == -1)
                    {
                        return Json(new { success = false, message = "No estás matriculado en este curso." });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Error al renunciar al curso." });
                    }
                }

                return Json(new { success = false, message = "Error en el servidor." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }

        // GET: Estudiantes/Calificaciones - Ver calificaciones
        [Seguridad]
        public async Task<IActionResult> Calificaciones()
        {
            try
            {
                int? idUsuario = HttpContext.Session.GetInt32("ConsecutivoUsuario");
                if (idUsuario == null)
                {
                    return RedirectToAction("Login", "Home");
                }

                using var http = _factory.CreateClient();
                var url = _configuration["Valores:UrlAPI"] + $"Estudiantes/Calificaciones/{idUsuario}";
                var response = await http.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var calificaciones = await response.Content.ReadFromJsonAsync<List<CalificacionCursoVM>>();
                    return View(calificaciones ?? new List<CalificacionCursoVM>());
                }

                TempData["Error"] = "No se pudieron cargar las calificaciones.";
                return View(new List<CalificacionCursoVM>());
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al cargar calificaciones: {ex.Message}";
                return View(new List<CalificacionCursoVM>());
            }
        }
    }
}
