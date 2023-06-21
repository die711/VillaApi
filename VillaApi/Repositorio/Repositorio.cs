using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VillaApi.Datos;
using VillaApi.Repositorio.IRepositorio;

namespace VillaApi.Repositorio;

public class Repositorio<T> : IRepositorio<T> where T : class
{
    private readonly ApplicationDbContext _db;
    internal DbSet<T> dbSet;

    public Repositorio(ApplicationDbContext db)
    {
        _db = db;
        this.dbSet = _db.Set<T>();

    }
    
    public Task Crear(T entidad)
    {
        throw new NotImplementedException();
    }

    public async Task<List<T>> ObtenerTodos(Expression<Func<T, bool>>? filtro = null)
    {
        IQueryable<T> query = dbSet;

        return await query.ToListAsync();

    }
}