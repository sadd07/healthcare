using Healthcare.Models;

namespace Healthcare.Dto;

public class DetailDoctorDto
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public ICollection<string> Schedules { get; set; } = new List<string>();
}
