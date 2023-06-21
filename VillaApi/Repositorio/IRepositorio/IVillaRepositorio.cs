using VillaApi.Models;

namespace VillaApi.Repositorio.IRepositorio;

public interface IVillaRepositorio : IRepositorio<Villa>
{
    Task<Villa> Actualizar(Villa villa);
}