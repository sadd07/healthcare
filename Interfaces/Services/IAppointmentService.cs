using System.Collections;
using Healthcare.Dto;
using Healthcare.Models;

namespace Healthcare.Interfaces.Services;

public interface IAppointmentService
{
    Task<IEnumerable<PatientDto>> GetAllPatients();
    Task<IEnumerable<AppointmentDto>> GetAllAppointments();
    Task<(string Message, int Code)> CreateAppointment(CreateAppointmentDto data);
    Task<bool> DeleteAppointment(int id);
}