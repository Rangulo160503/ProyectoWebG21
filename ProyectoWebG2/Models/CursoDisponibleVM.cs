namespace ProyectoWebG2.Models
{
    public class CursoDisponibleVM
    {
        public int IdCurso { get; set; }
        public string NombreCurso { get; set; } = string.Empty;
        public DateTime FechaDeInicio { get; set; }
        public DateTime FechaDeFinalizacion { get; set; }
        public int DuracionCurso { get; set; }
        public int CuposDisponibles { get; set; }
        public string Categoria { get; set; } = string.Empty;
        public string Modalidad { get; set; } = string.Empty;
        public int? IdInstructor { get; set; }
        public string NombreInstructor { get; set; } = string.Empty;
        public List<HorarioVM> Horarios { get; set; } = new List<HorarioVM>();
    }
}
