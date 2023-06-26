namespace VillaApi.Models.Dto;

public class LoginIdentityResponseDto
{
    public UsuarioDto Usuario { get; set; }
    public string Token { get; set; }
    public string Rol { get; set; }
    
}