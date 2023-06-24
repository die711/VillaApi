using System.Net;
using Microsoft.AspNetCore.Mvc;
using VillaApi.Models;
using VillaApi.Models.Dto;
using VillaApi.Repositorio.IRepositorio;

namespace VillaApi.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersionNeutral]
public class UsuarioController : Controller
{
    private readonly IUsuarioRepositorio _usuarioRepo;
    private APIResponse _response;

    public UsuarioController(IUsuarioRepositorio usuarioRepo)
    {
        _usuarioRepo = usuarioRepo;
        _response = new();
    }

    // GET
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto modelo)
    {
        var loginResponse = await _usuarioRepo.Login(modelo);
        if (loginResponse.Usuario == null || string.IsNullOrEmpty(loginResponse.Token))
        {
            _response.EstatusCode = HttpStatusCode.BadRequest;
            _response.IsExitoso = false;
            _response.ErrorMessages.Add("UserName o Password son Incorrectos");
            return BadRequest(_response);
        }

        _response.IsExitoso = true;
        _response.EstatusCode = HttpStatusCode.OK;
        _response.Resultado = loginResponse;
        return Ok(_response);
    }

    [HttpPost("registrar")]
    public async Task<IActionResult> Registrar([FromBody] RegistroRequestDto modelo)
    {
        bool isUsuarioUnico = _usuarioRepo.IsUsuarioUnico(modelo.UserName);

        if (!isUsuarioUnico)
        {
            _response.IsExitoso = false;
            _response.EstatusCode = HttpStatusCode.BadRequest;
            _response.ErrorMessages.Add("Usuario ya existe");
            return BadRequest(_response);
        }

        var usuario = await _usuarioRepo.Registrar(modelo);
        if (usuario == null)
        {
            _response.EstatusCode = HttpStatusCode.BadRequest;
            _response.IsExitoso = false;
            _response.ErrorMessages.Add("Error al registrar Usuario!");
            return BadRequest(_response);
        }

        _response.EstatusCode = HttpStatusCode.OK;
        _response.IsExitoso = true;
        return Ok(_response);
    }
}