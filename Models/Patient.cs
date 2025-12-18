using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Healthcare.Models;

[Table("patients")]
public class Patient : Model
{
    [Required]
    [StringLength(200)]
    public required string Name { get; set; }
}