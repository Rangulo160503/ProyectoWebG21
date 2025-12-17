namespace ProyectoWebG2.Models
{
    public class CalificacionVM
    {
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string NombreCurso { get; set; }
        public double? Calificacion { get; set; }  // Nullable para soportar NULL en BD
        public string Estado_Curso { get; set; }
        public string Fecha_Inicio { get; set; }
        public string Fecha_Finalizacion { get; set; }
    }
}