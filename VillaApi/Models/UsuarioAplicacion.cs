using Microsoft.AspNetCore.Identity;

namespace VillaApi.Models;

public class UsuarioAplicacion : IdentityUser
{
    public string Nombres { get; set; }
}