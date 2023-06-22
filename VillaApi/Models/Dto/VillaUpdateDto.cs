using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace VillaApi.Models.Dto;

public class VillaUpdateDto
{
    [Required]
    public int Id { get; set; }
    [Required]
    [MaxLength(30)]
    public string Nombre { get; set; }
    public string Detalle { get; set; }
    [Required]
    public double Tarifa { get; set; }
    [Required]
    public int Ocupantes { get; set; }
    public int MetrosCuadrados { get; set; }
    [Required]
    public string ImageUrl { get; set; }
    public string Amenidad { get; set; }
}