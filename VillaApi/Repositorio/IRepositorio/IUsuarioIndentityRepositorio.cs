using VillaApi.Models;
using VillaApi.Models.Dto;

namespace VillaApi.Repositorio.IRepositorio;

public interface IUsuarioIdentityRepositorio
{
    bool IsUsuarioUnico(string userName);

    Task<LoginIdentityResponseDto> Login(LoginRequestDto loginRequestDto);

    Task<UsuarioDto> Registrar(RegistroRequestDto registroRequestDto);


}