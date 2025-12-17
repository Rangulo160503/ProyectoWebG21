using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Dapper;

namespace ProyectoWebG2Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CursosController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public CursosController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("BDConnection")!;
        }

        // GET api/Cursos - Obtener todos los cursos
        [HttpGet]
        public async Task<IActionResult> GetAllCursos()
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var cursos = await connection.QueryAsync<dynamic>(
                    "SELECT c.*, ISNULL(u.Nombre + ' ' + u.Apellidos, 'Sin asignar') AS NombreInstructor FROM dbo.Curso c LEFT JOIN dbo.Usuario u ON c.IdInstructor = u.IdUsuario ORDER BY c.NombreCurso"
                );
                return Ok(cursos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener cursos", error = ex.Message });
            }
        }

        // GET Cursos/Disponibles - Cursos con cupos
        [HttpGet("Disponibles")]
        public async Task<IActionResult> GetCursosDisponibles()
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var cursos = await connection.QueryAsync<dynamic>(
                    "dbo.ObtenerCursosDisponibles",
                    commandType: System.Data.CommandType.StoredProcedure
                );

                return Ok(cursos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener cursos disponibles", error = ex.Message });
            }
        }

        // GET Cursos/Horarios/{idCurso}
        [HttpGet("Horarios/{idCurso}")]
        public async Task<IActionResult> GetHorariosCurso(int idCurso)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var horarios = await connection.QueryAsync<dynamic>(
                    "dbo.ObtenerHorariosCurso",
                    new { IdCurso = idCurso },
                    commandType: System.Data.CommandType.StoredProcedure
                );

                return Ok(horarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener horarios", error = ex.Message });
            }
        }
    }
}
