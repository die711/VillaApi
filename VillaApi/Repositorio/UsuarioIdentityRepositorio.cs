using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using VillaApi.Datos;
using VillaApi.Models;
using VillaApi.Models.Dto;
using VillaApi.Repositorio.IRepositorio;

namespace VillaApi.Repositorio;

public class UsuarioIdentityRepositorio : IUsuarioIdentityRepositorio
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<UsuarioAplicacion> _userManager;
    private readonly IMapper _mapper;
    private readonly RoleManager<IdentityRole> _roleManager;
    private string? secretKey;


    public UsuarioIdentityRepositorio(ApplicationDbContext db, IConfiguration configuration,
        UserManager<UsuarioAplicacion> userManager, IMapper mapper, RoleManager<IdentityRole> roleManager
    )
    {
        _db = db;
        _userManager = userManager;
        _mapper = mapper;
        _roleManager = roleManager;
        secretKey = configuration.GetValue<string>("ApiSettings:Secret");
    }

    public bool IsUsuarioUnico(string userName)
    {
        var usuario = _db.UsuariosAplicacion.FirstOrDefault(u => u.UserName.ToLower() == userName.ToLower());

        if (usuario == null)
        {
            return true;
        }

        return false;
    }

    public async Task<LoginIdentityResponseDto> Login(LoginRequestDto loginRequestDto)
    {
        var usuario = await _db.UsuariosAplicacion.FirstOrDefaultAsync(u =>
            u.UserName.ToLower() == loginRequestDto.UserName.ToLower());

        bool isValido = await _userManager.CheckPasswordAsync(usuario, loginRequestDto.Password);

        if (usuario == null || isValido == false)
        {
            return new LoginIdentityResponseDto()
            {
                Token = "",
                Usuario = null
            };
        }

        var roles = await _userManager.GetRolesAsync(usuario);

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secretKey);
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, usuario.UserName),
                new Claim(ClaimTypes.Role, roles.FirstOrDefault())
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        LoginIdentityResponseDto loginResponseDto = new()
        {
            Token = tokenHandler.WriteToken(token),
            Usuario = _mapper.Map<UsuarioDto>(usuario)
        };

        return loginResponseDto;
    }

    public async Task<UsuarioDto> Registrar(RegistroRequestDto registroRequestDto)
    {
        UsuarioAplicacion usuario = new()
        {
            UserName = registroRequestDto.UserName,
            Email = registroRequestDto.UserName,
            NormalizedEmail = registroRequestDto.UserName.ToUpper(),
            Nombres = registroRequestDto.Nombres,
         
        };

        try
        {
            var resultado = await _userManager.CreateAsync(usuario, registroRequestDto.Password);

            if (resultado.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync("admin"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("admin"));

                }
                
                await _userManager.AddToRoleAsync(usuario, "admin");
                var usuarioAp = _db.UsuariosAplicacion.FirstOrDefault(u => u.UserName == registroRequestDto.UserName);

                return _mapper.Map<UsuarioDto>(usuarioAp);
            }
            
        }
        catch (Exception ex)
        {
            throw;
        }

        return new UsuarioDto();
    }
}