using Healthcare.Models;

namespace Healthcare.Dto;

public class DoctorScheduleDto
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public List<ScheduleDto> Schedules { get; set; } = new List<ScheduleDto>();
}
