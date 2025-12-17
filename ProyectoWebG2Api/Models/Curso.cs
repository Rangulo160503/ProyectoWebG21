namespace ProyectoWebG2Api.Models
{
    public class Curso
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Duracion { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Modalidad { get; set; } = string.Empty;
        public string FechaInicio { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
    }
}
