using System.ComponentModel.DataAnnotations;

public class UsuarioModel
{
    public int ConsecutivoUsuario { get; set; }

    [Required]
    public string Cedula { get; set; } = string.Empty;

    [Required]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    public string Apellidos { get; set; } = string.Empty;

    [Required]
    public string Telefono { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string CorreoElectronico { get; set; } = string.Empty;

    [Required, DataType(DataType.Password)]
    public string Contrasena { get; set; } = string.Empty;

    [Required, DataType(DataType.Password)]
    [Compare("Contrasena", ErrorMessage = "Las contraseñas no coinciden.")]
    public string ConfirmarContrasena { get; set; } = string.Empty;

    public bool Estado { get; set; }
    public int ConsecutivoPerfil { get; set; }
    public string NombrePerfil { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;


    public int Rol { get; set; }
}
