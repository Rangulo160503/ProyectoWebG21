using Microsoft.AspNetCore.Mvc;
using ProyectoWebG2.Models;
using System.Net.Http.Json;
using System.Security.Cryptography;

namespace ProyectoWebG2.Controllers
{
    public class InstructoresController : Controller
    {
        private readonly IHttpClientFactory _factory;
        public InstructoresController(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _factory.CreateClient("api");
            var instructores = await client.GetFromJsonAsync<List<InstructorListadoVM>>("admin/instructores");
            return View(instructores ?? new List<InstructorListadoVM>());
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
                IdInstructor = vm.IdInstructor,
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

        public async Task<IActionResult> Editar(int id)
        {
            var client = _factory.CreateClient("api");
            var instructor = await client.GetFromJsonAsync<InstructorListadoVM>($"admin/instructores/{id}");

            if (instructor == null)
            {
                TempData["Error"] = "El instructor no existe.";
                return RedirectToAction("Index");
            }

            var vm = new InstructorVM
            {
                IdInstructor = instructor.IdInstructor,
                Cedula = instructor.Cedula,
                Nombre = instructor.Nombre,
                Apellidos = instructor.Apellidos,
                Telefono = instructor.Telefono,
                Correo = instructor.Correo
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(int id, InstructorVM vm)
        {
            if (id != vm.IdInstructor)
            {
                TempData["Error"] = "Los datos del instructor no coinciden.";
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var client = _factory.CreateClient("api");
            var instructor = await client.GetFromJsonAsync<InstructorListadoVM>($"admin/instructores/{id}");

            if (instructor == null)
            {
                TempData["Error"] = "El instructor no existe.";
                return RedirectToAction("Index");
            }

            var payload = new InstructorVM
            {
                Cedula = vm.Cedula,
                Nombre = vm.Nombre,
                Apellidos = vm.Apellidos,
                Telefono = vm.Telefono,
                Correo = vm.Correo,
                ContrasenaHash = string.IsNullOrWhiteSpace(vm.ContrasenaHash) ? string.Empty : HashPassword(vm.ContrasenaHash)
            };

            var res = await client.PutAsJsonAsync($"admin/instructores/{id}", payload);

            if (res.IsSuccessStatusCode)
            {
                TempData["Mensaje"] = "Instructor actualizado correctamente.";
                return RedirectToAction("Index");
            }

            TempData["Error"] = "No se pudo actualizar el instructor.";
            return View(vm);
        }

        public async Task<IActionResult> Eliminar(int id)
        {
            var client = _factory.CreateClient("api");
            var instructor = await client.GetFromJsonAsync<InstructorListadoVM>($"admin/instructores/{id}");

            if (instructor == null)
            {
                TempData["Error"] = "El instructor no existe.";
                return RedirectToAction("Index");
            }

            return View(instructor);
        }

        [HttpPost]
        public async Task<IActionResult> EliminarConfirmed(int id)
        {
            var client = _factory.CreateClient("api");
            var instructor = await client.GetFromJsonAsync<InstructorListadoVM>($"admin/instructores/{id}");

            if (instructor == null)
            {
                TempData["Error"] = "El instructor ya no existe.";
                return RedirectToAction("Index");
            }

            var res = await client.DeleteAsync($"admin/instructores/{id}");

            if (res.IsSuccessStatusCode)
            {
                TempData["Mensaje"] = "Instructor eliminado correctamente.";
                return RedirectToAction("Index");
            }

            TempData["Error"] = "No se pudo eliminar el instructor.";
            return RedirectToAction("Eliminar", new { id });
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
