using Microsoft.AspNetCore.Mvc;
using ProyectoWebG2.Models;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;

namespace ProyectoWebG2.Controllers
{
    public class InstructoresController : Controller
    {
        private readonly IHttpClientFactory _factory;
        public InstructoresController(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(InstructorVM vm)
        {
            // Hash de la contraseña igual que en el registro de usuario
            string contrasenaHash = HashPassword(vm.ContrasenaHash);
            var client = _factory.CreateClient("api");
            var payload = new InstructorVM
            {
                Cedula = vm.Cedula,
                Nombre = vm.Nombre,
                Apellidos = vm.Apellidos,
                Telefono = vm.Telefono,
                Correo = vm.Correo,
                ContrasenaHash = contrasenaHash
            };
            var res = await client.PostAsJsonAsync("admin/instructores", payload);
            if (res.IsSuccessStatusCode)
            {
                TempData["Msg"] = "Instructor creado correctamente.";
                return RedirectToAction("Create");
            }
            TempData["Error"] = "No se pudo crear el instructor.";
            return View(vm);
        }

        // Hash igual que en HomeController
        private static string HashPassword(string password)
        {
            const int iterations = 100_000;
            const int saltSize = 16;
            const int keySize = 32;
            var salt = RandomNumberGenerator.GetBytes(saltSize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                iterations,
                HashAlgorithmName.SHA256,
                keySize
            );
            return $"v1${iterations}${Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";
        }
    }
}
