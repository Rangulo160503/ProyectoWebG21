using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ProyectoWebG2Api.Models;
using System.Data;

namespace ProyectoWebG2Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IConfiguration _cfg;
        public AdminController(IConfiguration cfg)
        {
            _cfg = cfg;
        }

   
        [HttpGet("matriculas")]
        public IActionResult GetMatriculas()
        {
            using var cn = new SqlConnection(_cfg["ConnectionStrings:BDConnection"]);
            var result = cn.Query("ObtenerTodasLasMatriculas", commandType: CommandType.StoredProcedure);
            return Ok(result);
        }

        [HttpPost("asignar-curso")]
        public IActionResult AsignarCurso([FromBody] AsignarCursoRequest req)
        {
            using var cn = new SqlConnection(_cfg["ConnectionStrings:BDConnection"]);
            var p = new DynamicParameters();
            p.Add("@IdCurso", req.IdCurso);
            p.Add("@IdInstructor", req.IdInstructor);
            cn.Execute("AsignarCursoAInstructor", p, commandType: CommandType.StoredProcedure);
            return Ok();
        }
        public class AsignarCursoRequest { public int IdCurso { get; set; } public int IdInstructor { get; set; } }




        [HttpGet("calificaciones")]
        public IActionResult GetCalificaciones()
        {
            using var cn = new SqlConnection(_cfg["ConnectionStrings:BDConnection"]);
            var result = cn.Query("ObtenerTodasLasCalificaciones", commandType: CommandType.StoredProcedure);
            return Ok(result);
        }

        [HttpGet("asistencias")]
        public IActionResult GetAsistencias()
        {
            using var cn = new SqlConnection(_cfg["ConnectionStrings:BDConnection"]);
            var result = cn.Query("ObtenerTodasLasAsistencias", commandType: CommandType.StoredProcedure);
            return Ok(result);
        }


        [HttpPost("instructores")]
        public IActionResult CrearInstructor([FromBody] CrearInstructorRequest req)
        {
            using var cn = new SqlConnection(_cfg["ConnectionStrings:BDConnection"]);
            var p = new DynamicParameters();
            p.Add("@Cedula", req.Cedula);
            p.Add("@Nombre", req.Nombre);
            p.Add("@Apellidos", req.Apellidos);
            p.Add("@Telefono", req.Telefono);
            p.Add("@Correo", req.Correo);
            p.Add("@ContrasenaHash", req.ContrasenaHash);
            p.Add("@IdRol", 3); 
            var id = cn.ExecuteScalar<int>("Registro", p, commandType: CommandType.StoredProcedure);
            return Ok(id);
        }
    }


    public class CrearInstructorRequest
    {
        public string Cedula { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string ContrasenaHash { get; set; } = string.Empty;
    }
}
