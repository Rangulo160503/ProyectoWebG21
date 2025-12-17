namespace ProyectoWebG2.Models
{
    public class CursoMatriculadoVM
    {
        public int IdMatricula { get; set; }
        public int IdCurso { get; set; }
        public string NombreCurso { get; set; } = string.Empty;
        public DateTime FechaDeInicio { get; set; }
        public DateTime FechaDeFinalizacion { get; set; }
        public int DuracionCurso { get; set; }
        public string Categoria { get; set; } = string.Empty;
        public string Modalidad { get; set; } = string.Empty;
        public DateTime FechaMatricula { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string NombreInstructor { get; set; } = string.Empty;
        public string DiaSemana { get; set; } = string.Empty;
        public TimeSpan? HoraInicio { get; set; }
        public TimeSpan? HoraFin { get; set; }
    }
}
