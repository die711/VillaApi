using VillaApi.Datos;
using VillaApi.Models;
using VillaApi.Repositorio.IRepositorio;

namespace VillaApi.Repositorio;

public class NumeroVillaRepositorio : Repositorio<NumeroVilla>, INumeroVillaRepositorio
{
    public NumeroVillaRepositorio(ApplicationDbContext db) : base(db)
    {
    }

    public async Task<NumeroVilla> Actualizar(NumeroVilla entidad)
    {
        entidad.FechaActualizacion = DateTime.Now;
        dbSet.Update(entidad);
        await Grabar();
        return entidad;
    }
}