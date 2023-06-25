using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VillaApi.Datos;
using VillaApi.Models.Especificaciones;
using VillaApi.Repositorio.IRepositorio;

namespace VillaApi.Repositorio;

public class Repositorio<T> : IRepositorio<T> where T : class
{
    private readonly ApplicationDbContext _db;
    internal DbSet<T> dbSet;

    public Repositorio(ApplicationDbContext db)
    {
        _db = db;
        dbSet = _db.Set<T>();
    }
    
    public async Task Crear(T entidad)
    {
        await dbSet.AddAsync(entidad);
        await Grabar();
    }

    public async Task<List<T>> ObtenerTodos(Expression<Func<T, bool>>? filtro = null, string? incluirPropiedades = null)
    {
        IQueryable<T> query = dbSet;

        if (filtro != null)
            query = query.Where(filtro);

        if (incluirPropiedades != null)
        {
            foreach (var propiedad in incluirPropiedades.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(propiedad);
            }
        }

        return await query.ToListAsync();
    }

    public PagedList<T> ObtenerTodosPaginado(Parametros parametros, Expression<Func<T, bool>>? filtro = null, string? incluirPropiedades = null)
    {
        IQueryable<T> query = dbSet;

        if (filtro != null)
            query = query.Where(filtro);

        if (incluirPropiedades != null)
        {
            foreach (var propiedad in incluirPropiedades.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(propiedad);
            }
        }

        return PagedList<T>.ToPagedList(query, parametros.PageNumber, parametros.PageSize);
    }

    public async Task<T?> Obtener(Expression<Func<T, bool>>? filtro = null, bool tracked = true, string? incluirPropiedades = null)
    {
        IQueryable<T> query = dbSet;
        if (!tracked)
            query = query.AsNoTracking();

        if (filtro != null)
            query = query.Where(filtro);

        if (incluirPropiedades != null)
        {
            foreach (var propiedad in incluirPropiedades.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(propiedad);
            }
        }

        return await query.FirstOrDefaultAsync();
    }

    public async Task Remove(T entidad)
    {
        dbSet.Remove(entidad);
        await Grabar();
    }

    public  async Task Grabar()
    {
        await _db.SaveChangesAsync();
    }

   
}