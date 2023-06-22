using VillaApi.Models;

namespace VillaApi.Repositorio.IRepositorio;

public interface INumeroVillaRepositorio : IRepositorio<NumeroVilla>
{
    Task<NumeroVilla> Actualizar(NumeroVilla entidad);
}