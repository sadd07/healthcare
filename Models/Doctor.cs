using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Healthcare.Models;

[Table("doctors")]
public class Doctor : Model
{
    [Required]
    [StringLength(200)]
    public required string Name { get; set; }
}