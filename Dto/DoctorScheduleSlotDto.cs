using Healthcare.Models;

namespace Healthcare.Dto;

public class DoctorScheduleSlotDto
{
    public required int Id { get; set; }
    public required string Name { get; set; }

    public ICollection<string> Slots { get; set; } = new List<string>();
}
