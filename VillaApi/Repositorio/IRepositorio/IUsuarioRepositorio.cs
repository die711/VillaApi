using VillaApi.Models;
using VillaApi.Models.Dto;

namespace VillaApi.Repositorio.IRepositorio;

public interface IUsuarioRepositorio
{
    bool IsUsuarioUnico(string userName);

    Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);

    Task<Usuario?> Registrar(RegistroRequestDto registroRequestDto);


}