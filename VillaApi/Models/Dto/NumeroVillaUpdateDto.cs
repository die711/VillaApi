using System.ComponentModel.DataAnnotations;

namespace VillaApi.Models.Dto;

public class NumeroVillaUpdateDto
{
    [Required]
    public int VillaNo { get; set; }
    public int VillaId { get; set; }
    public string DetallesEspeciales { get; set; }
}