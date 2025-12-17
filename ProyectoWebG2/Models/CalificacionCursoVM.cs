namespace ProyectoWebG2.Models
{
    public class CalificacionCursoVM
    {
        public string NombreCurso { get; set; } = string.Empty;
        public decimal? Calificacion { get; set; }
        public string EstadoCurso { get; set; } = string.Empty;
        public DateTime? FechaFinalizacion { get; set; }
    }
}