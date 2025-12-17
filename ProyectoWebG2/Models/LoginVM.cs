// Models/LoginVM.cs
using System.ComponentModel.DataAnnotations;

public class LoginVM
{
    [Required, EmailAddress]
    public string CorreoElectronico { get; set; } = string.Empty;

    [Required, DataType(DataType.Password)]
    public string Contrasena { get; set; } = string.Empty;
}
