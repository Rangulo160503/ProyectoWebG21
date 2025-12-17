using System.ComponentModel.DataAnnotations;

namespace ProyectoWebG2Api.Models
{
    public class SeguridadRequestModel
    {
        [Required]
        public int ConsecutivoUsuario { get; set; }
        [Required]
        public string Contrasenna { get; set; } = string.Empty;
    }
}
