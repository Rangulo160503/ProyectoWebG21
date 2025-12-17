using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ProyectoWebG2Api.Models;
using System.Data;
using System.Security.Cryptography;

namespace ProyectoWebG2Api.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _environment;

        public HomeController(IConfiguration configuration, IHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        
        [HttpPost("Registro")]
        public async Task<IActionResult> Registro([FromBody] RegistroUsuarioRequestModel usuario)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!string.Equals(usuario.Contrasena, usuario.ConfirmarContrasena))
                return BadRequest("Las contraseñas no coinciden.");

            var contrasenaHash = HashPassword(usuario.Contrasena);

            using var cn = new SqlConnection(_configuration.GetConnectionString("BDConnection"));
            var p = new DynamicParameters();
            p.Add("@Cedula", usuario.Cedula);
            p.Add("@Nombre", usuario.Nombre);
            p.Add("@Apellidos", usuario.Apellidos);
            p.Add("@Telefono", usuario.Telefono);
            p.Add("@Correo", usuario.CorreoElectronico);
            p.Add("@ContrasenaHash", contrasenaHash);

             if (usuario.idRol > 0)
            {
                p.Add("@IdRol", usuario.idRol);
            }

            var resultado = await cn.ExecuteScalarAsync<int>(
                "dbo.Registro",
                p,
                commandType: CommandType.StoredProcedure
            );

            return resultado switch
            {
                > 0 => Ok(resultado),
                -1 => Conflict("La cedula ya existe"),
                -2 => Conflict("El correo ya existe"),
                _ => StatusCode(500, "No se pudo registrar")
            };
        }

        //  INICIAR SESIÓN 
        [HttpPost("IniciarSesion")]
        public async Task<IActionResult> IniciarSesion([FromBody] LoginRequest usuario)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            using var cn = new SqlConnection(_configuration.GetConnectionString("BDConnection"));

           
            const string sql = @"
    SELECT TOP 1
        IdUsuario       AS ConsecutivoUsuario,
        Nombre,
        Apellidos,
        Correo          AS CorreoElectronico,
        ContrasenaHash,
        IdRol           AS Rol  -- Agregamos el ID del rol
    FROM dbo.Usuario
    WHERE Correo = @Correo;";

            var row = await cn.QueryFirstOrDefaultAsync<UsuarioDbRow>(sql, new { Correo = usuario.CorreoElectronico });

            if (row is null)
                return Unauthorized("Credenciales inválidas.");

          
            var ok = VerifyPassword(usuario.Contrasena, row.ContrasenaHash);
            if (!ok)
                return Unauthorized("Credenciales inválidas.");

            
            string nombrePerfil = row.Rol switch
            {
                1 => "Administrador",
                2 => "Estudiante",
                3 => "Instructor",
                _ => "Desconocido"
            };

            
            var respuesta = new SesionResponse
            {
                ConsecutivoUsuario = row.ConsecutivoUsuario,
                Nombre = row.Nombre,
                Apellidos = row.Apellidos,
                CorreoElectronico = row.CorreoElectronico,
                NombrePerfil = nombrePerfil, 
                Rol = row.Rol  
            };

            return Ok(respuesta);
        }


       
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

        private static bool VerifyPassword(string password, string stored)
        {
            
            var parts = stored.Split('$');
            if (parts.Length != 4 || parts[0] != "v1") return false;

            var iterations = int.Parse(parts[1]);
            var salt = Convert.FromBase64String(parts[2]);
            var hash = Convert.FromBase64String(parts[3]);

            var testHash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                iterations,
                HashAlgorithmName.SHA256,
                hash.Length
            );

            return CryptographicOperations.FixedTimeEquals(hash, testHash);
        }


        private sealed class UsuarioDbRow
        {
            public int ConsecutivoUsuario { get; set; }
            public string Nombre { get; set; } = "";
            public string Apellidos { get; set; } = "";
            public string CorreoElectronico { get; set; } = "";
            public string ContrasenaHash { get; set; } = "";
            public int Rol { get; set; }
        }

        private sealed class SesionResponse
        {
            public int ConsecutivoUsuario { get; set; }
            public string Nombre { get; set; } = "";
            public string Apellidos { get; set; } = "";
            public string CorreoElectronico { get; set; } = "";
            public string NombrePerfil { get; set; } = "";
            public int Rol { get; set; }
        }

        public sealed class LoginRequest
        {
            public string CorreoElectronico { get; set; } = string.Empty;
            public string Contrasena { get; set; } = string.Empty;
        }
    }
}













