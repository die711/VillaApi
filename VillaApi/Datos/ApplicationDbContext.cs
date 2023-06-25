using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VillaApi.Models;

namespace VillaApi.Datos;

public class ApplicationDbContext : IdentityDbContext<UsuarioAplicacion>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
    {
        
    }

    public DbSet<UsuarioAplicacion> UsuariosAplicacion { get; set; }
    public DbSet<Villa> Villas { get; set; }
    public DbSet<NumeroVilla> NumeroVillas  { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Villa>().HasData(
            new Villa()
            {
                Id =1,
                Nombre = "Villa Real",
                Detalle = "Detalle de la villa",
                ImageUrl = "",
                Ocupantes = 5,
                MetrosCuadrados = 50,
                Tarifa = 50,
                Amenidad = "",
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now
            },
            new Villa()
            {
                Id = 2,
                Nombre = "Premium Vista ala piscina",
                Detalle = "Detalle de la villa",
                ImageUrl = "",
                Ocupantes = 5,
                MetrosCuadrados = 50,
                Amenidad = "",
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now
            }
        );
    }
}