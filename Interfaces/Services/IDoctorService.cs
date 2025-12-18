using System.Collections;
using Healthcare.Dto;

namespace Healthcare.Interfaces.Services;

public interface IDoctorService
{
    Task<IEnumerable<DoctorDto>> GetAllDoctors();
    Task<DetailDoctorDto?> GetDoctorById(int id);
    Task<DoctorScheduleSlotDto?> GetDoctorScheduleSlots(GetDoctorScheduleSlotsDto data);
}