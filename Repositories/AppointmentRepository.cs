using Healthcare.Dto;
using Healthcare.Enums;
using Healthcare.Interfaces.Repositories;
using Healthcare.Models;
using Microsoft.EntityFrameworkCore;

namespace Healthcare.Repositories;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly AppDbContext _context;

    public AppointmentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AppointmentDto>> GetAll()
    {
        return await _context.Appointments.Select(p => new AppointmentDto
        {
            Id = p.Id,
            PatientId = p.PatientId,
            ScheduleId = p.ScheduleId,
            Day = (Days)p.Day,
            Start = p.Start,
            Duration = p.Duration,
        }).ToListAsync();
    }
    
    public async Task<AppointmentDto?> GetById(int id)
    {
        var appointment = await _context.Appointments.FirstOrDefaultAsync(p => p.Id == id);
        if (appointment == null) return null;

        return new AppointmentDto
        {
            Id = appointment.Id,
            PatientId = appointment.PatientId,
            ScheduleId = appointment.ScheduleId,
            Day = (Days)appointment.Day,
            Start = appointment.Start,
            Duration = appointment.Duration,
        };
    }

    public async Task<IEnumerable<AppointmentDto>> GetByScheduleId(int scheduleId)
    {
        return await _context.Appointments.Select(p => new AppointmentDto
        {
            Id = p.Id,
            PatientId = p.PatientId, 
            ScheduleId = p.ScheduleId, 
            Day = (Days)p.Day, 
            Start = p.Start, 
            Duration = p.Duration, 
        }).ToListAsync();
    }

    public async Task<CreateAppointmentDto> Create(CreateAppointmentDto data)
    {
        var appointment = new Appointment
        {
            PatientId = data.PatientId,
            ScheduleId = data.ScheduleId,
            Day = (int)data.Day,
            Start = data.Start,
            Duration = data.Duration,
        };
        await _context.Appointments.AddAsync(appointment);
        await _context.SaveChangesAsync();

        data.Id = appointment.Id;

        return data;
    }

    public async Task<bool> Delete(int id)
    {
        var appointment = await _context.Appointments
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(a => a.Id == id);

        if (appointment == null)
            return false;
  
        _context.Appointments.Remove(appointment);
        await _context.SaveChangesAsync();

        return true;
    }
}