using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ProyectoWebG2Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstudiantesController : ControllerBase
    {
        private readonly IConfiguration _cfg;

        public EstudiantesController(IConfiguration cfg)
        {
            _cfg = cfg;
        }

        // GET: api/Estudiantes/CursosDisponibles
        [HttpGet("CursosDisponibles")]
        public IActionResult GetCursosDisponibles()
        {
            try
            {
                using var cn = new SqlConnection(_cfg["ConnectionStrings:BDConnection"]);
                var result = cn.Query("ObtenerCursosDisponibles", commandType: CommandType.StoredProcedure);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // GET: api/Estudiantes/HorariosCurso/{idCurso}
        [HttpGet("HorariosCurso/{idCurso}")]
        public IActionResult GetHorariosCurso(int idCurso)
        {
            try
            {
                using var cn = new SqlConnection(_cfg["ConnectionStrings:BDConnection"]);
                var p = new DynamicParameters();
                p.Add("@IdCurso", idCurso);
                var result = cn.Query("ObtenerHorariosCurso", p, commandType: CommandType.StoredProcedure);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // POST: api/Estudiantes/Matricular
        [HttpPost("Matricular")]
        public IActionResult MatricularEstudiante([FromBody] MatricularRequest request)
        {
            try
            {
                using var cn = new SqlConnection(_cfg["ConnectionStrings:BDConnection"]);
                var p = new DynamicParameters();
                p.Add("@IdUsuario", request.IdUsuario);
                p.Add("@IdCurso", request.IdCurso);
                p.Add("@IdHorario", request.IdHorario);

                var result = cn.QueryFirstOrDefault<int>("MatricularEstudiante", p, commandType: CommandType.StoredProcedure);

                // Validaciones
                if (result == -1)
                {
                    return Ok(new { success = false, message = "Ya estás matriculado en este curso." });
                }

                if (result == 0)
                {
                    return Ok(new { success = false, message = "No se pudo realizar la matrícula." });
                }

                // Caso exitoso
                return Ok(new { success = true, message = "¡Matrícula realizada con éxito!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error en el servidor: " + ex.Message });
            }
        }

        // GET: api/Estudiantes/CursosMatriculados/{idUsuario}
        [HttpGet("CursosMatriculados/{idUsuario}")]
        public IActionResult GetCursosMatriculados(int idUsuario)
        {
            try
            {
                using var cn = new SqlConnection(_cfg["ConnectionStrings:BDConnection"]);
                var p = new DynamicParameters();
                p.Add("@IdUsuario", idUsuario);
                var result = cn.Query("ObtenerCursosMatriculados", p, commandType: CommandType.StoredProcedure);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // POST: api/Estudiantes/Desmatricular/{idUsuario}/{idCurso}
        [HttpPost("Desmatricular/{idUsuario}/{idCurso}")]
        public IActionResult DesmatricularEstudiante(int idUsuario, int idCurso)
        {
            try
            {
                using var cn = new SqlConnection(_cfg["ConnectionStrings:BDConnection"]);
                var p = new DynamicParameters();
                p.Add("@IdUsuario", idUsuario);
                p.Add("@IdCurso", idCurso);
                
                var result = cn.QueryFirstOrDefault<int>("DesmatricularEstudiante", p, commandType: CommandType.StoredProcedure);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // GET: api/Estudiantes/Calificaciones/{idUsuario}
        [HttpGet("Calificaciones/{idUsuario}")]
        public IActionResult GetCalificaciones(int idUsuario)
        {
            try
            {
                using var cn = new SqlConnection(_cfg["ConnectionStrings:BDConnection"]);
                var p = new DynamicParameters();
                p.Add("@IdUsuario", idUsuario);
                var result = cn.Query("ObtenerCalificacionesPorUsuario", p, commandType: CommandType.StoredProcedure);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }

    // Request model for enrollment
    public class MatricularRequest
    {
        public int IdUsuario { get; set; }
        public int IdCurso { get; set; }
        public int? IdHorario { get; set; }
    }
}
//prueba