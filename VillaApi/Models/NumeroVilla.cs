using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VillaApi.Models;

public class NumeroVilla
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int VillaNo { get; set; }
    
    [Required]
    public int VillaId { get; set; }
    
    public Villa Villa { get; set; }
    
    public string DetallesEspeciales { get; set; }
    
    public DateTime FechaCreacion { get; set; }
    
    public DateTime FechaActualizacion { get; set; }
}