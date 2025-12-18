using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Healthcare.Models;

[Table("schedules")]
public class Schedule : Model
{
    [Required]
     [ForeignKey(nameof(Doctor))]
    public required int DoctorId { get; set; }

    [Required]
    public required int DayId { get; set; }

    [Required]
    public required TimeOnly From { get; set; }

    [Required]
    public required TimeOnly To { get; set; }
}