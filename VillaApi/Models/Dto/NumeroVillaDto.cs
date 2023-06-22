using System.ComponentModel.DataAnnotations;

namespace VillaApi.Models.Dto;

public class NumeroVillaDto
{
    [Required]
    public int VillaNo { get; set; }
    public int VillaId { get; set; }
    public string DetallesEspeciales { get; set; }
    public VillaDto Villa { get; set; }
}