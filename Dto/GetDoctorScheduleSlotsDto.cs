using Healthcare.Enums;

namespace Healthcare.Dto;

public class GetDoctorScheduleSlotsDto
{
    public required int Id { get; set; }
    public required Days Day { get; set; }
    public required TimeOnly From { get; set; }
    public required TimeOnly To { get; set; }
    public required int Slot { get; set; }
}
