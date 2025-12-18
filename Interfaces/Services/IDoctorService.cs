using System.Collections;
using Healthcare.Dto;
using Healthcare.Models;

namespace Healthcare.Interfaces.Services;

public interface IDoctorService
{
    Task<IEnumerable<DoctorDto>> GetAllDoctors();
    Task<DetailDoctorDto?> GetDoctorById(int id);
    Task<DoctorScheduleSlotDto?> GetDoctorScheduleSlots(GetDoctorScheduleSlotsDto data);
    Task<(IEnumerable<DoctorDto>, 
        IEnumerable<ScheduleDto>,
        IEnumerable<PatientDto>
    )> Seeds();
}