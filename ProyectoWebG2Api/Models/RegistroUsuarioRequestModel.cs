using System.ComponentModel.DataAnnotations;

namespace ProyectoWebG2Api.Models
{
    public class RegistroUsuarioRequestModel
    {
        [Required] public string Cedula { get; set; } = string.Empty;
        [Required] public string Nombre { get; set; } = string.Empty;
        [Required] public string Apellidos { get; set; } = string.Empty;
        [Required] public string Telefono { get; set; } = string.Empty;
        [Required] public string CorreoElectronico { get; set; } = string.Empty;
        [Required] public string Contrasena { get; set; } = string.Empty;
        [Required] public string ConfirmarContrasena { get; set; } = string.Empty;
        [Required] public int idRol{ get; set; } = 2;

    }
}
