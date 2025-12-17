using System.ComponentModel.DataAnnotations;

namespace ProyectoWebG2.Models
{
    public class InstructorVM
    {
        public int IdInstructor { get; set; }

        [Required(ErrorMessage = "La cédula es obligatoria.")]
        public string Cedula { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Los apellidos son obligatorios.")]
        public string Apellidos { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono es obligatorio."), Phone]
        public string Telefono { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es obligatorio."), EmailAddress]
        public string Correo { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        public string ContrasenaHash { get; set; } = string.Empty;
    }
}
