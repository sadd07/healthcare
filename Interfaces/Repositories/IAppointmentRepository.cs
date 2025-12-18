using Healthcare.Dto;

namespace Healthcare.Interfaces.Repositories;

public interface IAppointmentRepository
{
    Task<IEnumerable<AppointmentDto>> GetAll();
    Task<AppointmentDto?> GetById(int id);
    Task<IEnumerable<AppointmentDto>> GetByScheduleId(int scheduleId);
    Task<CreateAppointmentDto> Create(CreateAppointmentDto data);
    Task<bool> Delete(int id);
}