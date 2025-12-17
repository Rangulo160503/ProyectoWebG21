using System.ComponentModel.DataAnnotations;

public class RecuperarVM
{
    [Required, EmailAddress]
    public string CorreoElectronico { get; set; } = string.Empty;
}