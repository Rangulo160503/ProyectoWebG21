using System.ComponentModel.DataAnnotations;

namespace ProyectoWebG2Api.Models
{
    public class RecuperarAccesoRequestModel
    {
        [Required, EmailAddress]
        public string CorreoElectronico { get; set; } = string.Empty;
    }
}