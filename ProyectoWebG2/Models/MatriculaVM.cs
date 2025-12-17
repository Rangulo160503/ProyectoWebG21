namespace ProyectoWebG2.Models
{
    public class MatriculaVM
    {
        public int IdMatricula { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string NombreCurso { get; set; } = string.Empty;
        public string FechaMatricula { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
    }
}
