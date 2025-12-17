using System.ComponentModel.DataAnnotations;

namespace ProyectoWebG2Api.Models
{
    public class LoginRequestModel
    {
        [Required, EmailAddress]
        public string CorreoElectronico { get; set; } = string.Empty;

        [Required]
        public string Contrasena { get; set; } = string.Empty;
    }
}