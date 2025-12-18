using Healthcare.Enums;

namespace Healthcare.Dto;

public class CreateAppointmentDto
{
    public int Id { get; set; }
    public required int DoctorId { get; set; }
    public required int PatientId { get; set; }
    public required Days Day { get; set; }
    public required TimeOnly Start { get; set; }
    public required int Duration { get; set; }
    public int ScheduleId { get; set; }

}
