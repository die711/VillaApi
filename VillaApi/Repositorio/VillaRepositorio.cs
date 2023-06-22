using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using VillaApi.Datos;
using VillaApi.Models;
using VillaApi.Repositorio.IRepositorio;

namespace VillaApi.Repositorio;

public class VillaRepositorio : Repositorio<Villa>, IVillaRepositorio
{
    public VillaRepositorio(ApplicationDbContext db) : base(db)
    {
        
    }
    
    public async Task<Villa> Actualizar(Villa entidad)
    {
        entidad.FechaActualizacion = DateTime.Now;
        dbSet.Update(entidad);
        await Grabar();
        return entidad;
    }

}