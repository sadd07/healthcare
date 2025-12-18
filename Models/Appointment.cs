using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Healthcare.Models;

[Table("appointments")]
public class Appointment : Model
{
    [Required]
     [ForeignKey(nameof(Patient))]
    public required int PatientId { get; set; }
    
    [Required]
     [ForeignKey(nameof(Schedule))]
    public required int ScheduleId { get; set; }
    
    [Required]
    public required int Day { get; set; }

    [Required]
    public required TimeOnly Start { get; set; }

    [Required]
    public required int Duration { get; set; }
    
}