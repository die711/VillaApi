using Microsoft.EntityFrameworkCore;
using VillaApi.Models;

namespace VillaApi.Datos;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
    {
        
    }

    public DbSet<Villa> Villas { get; set; }
    public DbSet<NumeroVilla> NumeroVillas  { get; set; }
   
}