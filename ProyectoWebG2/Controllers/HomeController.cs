using Microsoft.AspNetCore.Mvc;
using ProyectoWebG2.Models;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;

namespace ProyectoWebG2.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _factory;
        public HomeController(IConfiguration configuration, IHttpClientFactory factory)
        {
            _configuration = configuration;
            _factory = factory;
        }

        #region Iniciar Sesión

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginPost(LoginVM usuario)
        {
            using var http = _factory.CreateClient();
            var url = _configuration["Valores:UrlAPI"] + "Home/IniciarSesion";
            var res = await http.PostAsJsonAsync(url, usuario);

            if (!res.IsSuccessStatusCode)
            {
                TempData["Error"] = "Credenciales inválidas.";
                return View("Login", usuario);
            }

            // Recibe el usuario autenticado desde la API
            var loginResponse = await res.Content.ReadFromJsonAsync<UsuarioModel>();
            if (loginResponse != null)
            {
                HttpContext.Session.SetString("IsAuth", "1");
                HttpContext.Session.SetString("Email", loginResponse.CorreoElectronico ?? string.Empty);
                HttpContext.Session.SetInt32("ConsecutivoUsuario", loginResponse.ConsecutivoUsuario);
                HttpContext.Session.SetString("NombreUsuario", loginResponse.Nombre);
                HttpContext.Session.SetString("NombrePerfil", loginResponse.NombrePerfil);
                HttpContext.Session.SetInt32("Rol", loginResponse.Rol);
                
                // Redirigir según el rol
                if (loginResponse.Rol == 2) // Estudiante
                {
                    return RedirectToAction("Index", "Estudiantes");
                }
            }

            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region Crear Usuarios

        // GET: /registro
        [HttpGet("registro")]
        public IActionResult Registro()
        {
            return View("Registro", new UsuarioModel());
        }

        // POST: /registro
        [HttpPost("registro")]
        public async Task<IActionResult> RegistroPost(UsuarioModel vm)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Complete los campos requeridos.";
                return View("Registro", vm);
            }

            if (vm.Contrasena != vm.ConfirmarContrasena)
            {
                TempData["Error"] = "Las contraseñas no coinciden.";
                ModelState.AddModelError(nameof(vm.ConfirmarContrasena), "Debe coincidir con la contraseña.");
                return View("Registro", vm);
            }

            try
            {
                using var http = _factory.CreateClient();
                var url = _configuration["Valores:UrlAPI"] + "Home/Registro";

                var payload = new
                {
                    Cedula = vm.Cedula,
                    Nombre = vm.Nombre,
                    Apellidos = vm.Apellidos,
                    Telefono = vm.Telefono,
                    CorreoElectronico = vm.CorreoElectronico,
                    Contrasena = vm.Contrasena,
                    ConfirmarContrasena = vm.ConfirmarContrasena
                };

                var res = await http.PostAsJsonAsync(url, payload);

                if (!res.IsSuccessStatusCode)
                {
                   
                    var apiMsg = await res.Content.ReadAsStringAsync();
                    TempData["Error"] = !string.IsNullOrWhiteSpace(apiMsg) ? apiMsg : "No se pudo registrar. Verifique los datos o si ya existe el usuario/correo.";
                    return View("Registro", vm);
                }

                var idUsuario = await res.Content.ReadFromJsonAsync<int>();
                if (idUsuario > 0)
                {
                    TempData["Msg"] = "Registro exitoso. Ahora puedes iniciar sesión.";
                    return RedirectToAction("Index", "Home");
                }
                else if (idUsuario == -1)
                {
                    TempData["Error"] = "La cédula ya existe.";
                }
                else if (idUsuario == -2)
                {
                    TempData["Error"] = "El correo ya existe.";
                }
                else
                {
                    TempData["Error"] = "No se ha registrado la información.";
                }
                return View("Registro", vm);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Ocurrió un error inesperado: {ex.Message}";
                return View("Registro", vm);
            }
        }

        #endregion

      
        // RECUPERAR (GET/POST) 
        
        [HttpGet("recuperar")]
        public IActionResult Recuperar() => View();

        #region Recuperar Acceso

        [HttpGet]
        public IActionResult RecuperarAcceso()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RecuperarAcceso(UsuarioModel usuario)
        {
            using (var context = _factory.CreateClient())
            {
                var urlApi = _configuration["Valores:UrlAPI"] + "Home/RecuperarAcceso?CorreoElectronico=" + usuario.CorreoElectronico;
                var resultado = context.GetAsync(urlApi).Result;

                if (resultado.IsSuccessStatusCode)
                {
                    var datosApi = resultado.Content.ReadFromJsonAsync<UsuarioModel>().Result;

                    if (datosApi != null)
                        return RedirectToAction("Login", "Home");
                }

                ViewBag.Mensaje = "No se ha recuperado el acceso";
                return View();
            }
        }

        #endregion
        
        [HttpGet]
        public IActionResult Logout()
        {
            // Limpia toda la sesión
            HttpContext.Session.Clear();

            // Llévalo al login
            return RedirectToAction("Login", "Home");
        }

        [Seguridad]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [Seguridad]
        [HttpGet]
        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Home");
        }

    }
}

