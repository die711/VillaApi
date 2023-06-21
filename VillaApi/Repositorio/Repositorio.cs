using System.Linq.Expressions;
using VillaApi.Repositorio.IRepositorio;

namespace VillaApi.Repositorio;

public class Repositorio<T> : IRepositorio<T> where T : class
{
    public Task Crear(T entidad)
    {
        throw new NotImplementedException();
    }

    public Task<List<T>> ObtenerTodos(Expression<Func<T, bool>>? filtro = null)
    {
        throw new NotImplementedException();
    }
}