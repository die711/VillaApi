using System.Linq.Expressions;
using VillaApi.Datos;
using VillaApi.Models;
using VillaApi.Repositorio.IRepositorio;

namespace VillaApi.Repositorio;

public class VillaRepositorio : Repositorio<Villa>, IVillaRepositorio
{
    public VillaRepositorio(ApplicationDbContext db) : base(db)
    {
        
    }
    
    public Task<Villa> Actualizar(Villa villa)
    {
        throw new NotImplementedException();
    }

}