namespace ProyectoWebG2Api.Models
{
    public class InstructorListado
    {
        public int IdInstructor { get; set; }
        public string Cedula { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public int CursosAsignados { get; set; }
    }
}
