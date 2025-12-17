namespace ProyectoWebG2.Models
{
    public class AsistenciaVM
    {
        public int IdAsistencia { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string NombreCurso { get; set; } = string.Empty;
        public string Fecha { get; set; } = string.Empty;
        public string EstadoAsistencia { get; set; } = string.Empty;
    }
}
