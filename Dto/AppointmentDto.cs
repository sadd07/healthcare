using Healthcare.Enums;
using Healthcare.Models;

namespace Healthcare.Dto;

public class AppointmentDto
{
    public int Id { get; set; }
    public required int PatientId { get; set; }
    public required int ScheduleId { get; set; }
    public required Days Day { get; set; }
    public required TimeOnly Start { get; set; }
    public required int Duration { get; set; }

    public ICollection<ScheduleDto> Schedules { get; set; } = new List<ScheduleDto>();
}