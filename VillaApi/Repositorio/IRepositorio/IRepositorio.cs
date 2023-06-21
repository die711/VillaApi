using System.Linq.Expressions;

namespace VillaApi.Repositorio.IRepositorio;

public interface IRepositorio<T> where T : class
{
    Task Crear(T entidad);
    Task<List<T>> ObtenerTodos(Expression<Func<T, bool>>? filtro = null);





}