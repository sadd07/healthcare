using Healthcare.Dto;
using Healthcare.Enums;
using Healthcare.Exceptions;
using Healthcare.Interfaces.Repositories;
using Healthcare.Interfaces.Services;
using Healthcare.Models;

namespace Healthcare.Services;

public class AppointmentService : IAppointmentService
{
    private readonly Days _dayOfWeek;
    private readonly IDoctorRepository _doctor;
    private readonly IPatientRepository _patient;
    private readonly IAppointmentRepository _appointment;

    public AppointmentService(
        IDoctorRepository doctor,
        IPatientRepository patient,
        IAppointmentRepository appointment
    )
    {
        _doctor = doctor;
        _patient = patient;
        _appointment = appointment;
    }
    
    public async Task<IEnumerable<PatientDto>> GetAllPatients()
    {
         return await _patient.GetAll();
    }
    
    public async Task<IEnumerable<AppointmentDto>> GetAllAppointments()
    {
         return await _appointment.GetAll();
    }
    
    public async Task<(string Message, int Code)> CreateAppointment(CreateAppointmentDto data)
    {
        var doctor = await _doctor.GetSchedulesByDoctorId(data.DoctorId);
        if (doctor == null) return ("Doctor not found.", 1);

        var patient = await _patient.GetById(data.PatientId);
        if (patient == null) return ("Patient not found.", 1);

        var scheduleDay = doctor.Schedules
            .OrderBy(s => s.From)
            .Where(s => s.DayId == (int)data.Day)
            .FirstOrDefault();
        
        var schedule = doctor.Schedules
            .OrderBy(s => s.From)
            .Where(s => s.DayId == (int)data.Day)
            .Where(s =>
            {
                var from = TimeOnly.Parse(s.From);
                return from <= data.Start;
            })
            .FirstOrDefault();
        if (scheduleDay == null) return ("Doctor schedule not found.", 1);
       
        var end = data.Start.AddMinutes(data.Duration);
        if (schedule == null || TimeOnly.Parse(schedule.To) < end) return ($"Doctor schedule outside working hours.", 1);
        
        var appointments = await _appointment.GetByScheduleId(schedule.Id);
        if (appointments != null)
        {
            foreach (var appointment in appointments)
            {
                var _end = appointment.Start.AddMinutes(appointment.Duration);
                Console.WriteLine(appointment.Start);
                Console.WriteLine(data.Start);
                Console.WriteLine(_end);
                Console.WriteLine(end);
                if ((data.Start >= appointment.Start && data.Start < _end)
                    || (end > appointment.Start && end < _end))
                {
                    return ($"Overlap appointment with {appointment.Start.ToString("HH:mm tt")} - {_end.ToString("HH:mm tt")}", 10);
                }
            }
        }

        await _appointment.Create(new CreateAppointmentDto
        {
            DoctorId = doctor.Id,
            PatientId = patient.Id,
            ScheduleId = schedule.Id,
            Day = data.Day,
            Start = data.Start,
            Duration = data.Duration,
        });

        return ($"Success create appointment, Slot {data.Start} - {end} not available yet.", 0);
    }

    public async Task<bool> DeleteAppointment(int id)
    {
        var appointment = await _appointment.GetById(id);
        if (appointment == null) throw new BadRequestException("Appointment data not found.");

        var today = DateTime.Now;
        var dayId = GetDay(today.DayOfWeek);
        if (dayId == appointment.Day)
        {
            TimeOnly time = TimeOnly.FromDateTime(today);
            time = time.AddHours(2);

            if (time >= appointment.Start)
            {
                throw new ConflictException("Cancellation must be made at least 2 hours before the scheduled appointment time.");
            }
        }


        return await _appointment.Delete(id);   
    }

    protected Days GetDay(DayOfWeek day)
    {
        return day switch
        {
            DayOfWeek.Sunday => Days.Sunday,
            DayOfWeek.Monday => Days.Monday,
            DayOfWeek.Tuesday => Days.Tuesday,
            DayOfWeek.Wednesday => Days.Wednesday,
            DayOfWeek.Thursday => Days.Thursday,
            DayOfWeek.Friday => Days.Friday,
            DayOfWeek.Saturday => Days.Saturday,
        };
    }
}