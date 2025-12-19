using Healthcare.Dto;
using Healthcare.Enums;
using Healthcare.Interfaces.Repositories;
using Healthcare.Interfaces.Services;
using Healthcare.Models;

namespace Healthcare.Services;

public class DoctorService : IDoctorService
{
    private readonly Days _dayOfWeek;
    private readonly IDoctorRepository _doctor;
    private readonly IScheduleRepository _schedule;
    private readonly IPatientRepository _patient;
    private readonly IAppointmentRepository _appointment;

    public DoctorService(
        IDoctorRepository doctor,
        IScheduleRepository schedule,
        IPatientRepository patient,
        IAppointmentRepository appointment
    )
    {
        _doctor = doctor;
        _schedule = schedule;
        _patient = patient;
        _appointment = appointment;
    }
    
    public async Task<IEnumerable<DoctorDto>> GetAllDoctors()
    {
         return await _doctor.GetAll();
    }
    
    public async Task<DetailDoctorDto?> GetDoctorById(int id)
    {
        var doctor = await _doctor.GetSchedulesByDoctorId(id);
        if (doctor == null) return null;

        var schedules = doctor.Schedules
            .Select(s => $"{s.DayId} {s.From:h:mmtt} - {s.To:h:mmtt}")
            .ToList();
        
        return new DetailDoctorDto
        {
            Id = doctor.Id,
            Name = doctor.Name,
            Schedules = doctor.Schedules
                .OrderBy(s => s.DayId)
                .ThenBy(s => s.From)
                .Select(s => $"{GetDayName(s.DayId)} {s.From:h:mmtt} - {s.To:h:mmtt}")
                .ToList()
        };
    }

    public async Task<DoctorScheduleSlotDto?> GetDoctorScheduleSlots(GetDoctorScheduleSlotsDto data)
    {
        var doctor = await _doctor.GetSchedulesByDoctorId(data.Id);
        if (doctor == null) return null;

        var schedules = doctor.Schedules
            .OrderBy(s => s.From)
            .Where(s => s.DayId == (int)data.Day)
            .Where(s =>
            {
                var from = TimeOnly.Parse(s.From);
                var to = TimeOnly.Parse(s.To);
                return from >= data.From && to <= data.To;
            })
            .ToList();

        List<string> timeSlots = new List<string>();

        foreach (var schedule in schedules)
        {
            TimeOnly _start = TimeOnly.Parse(schedule.From);
            TimeOnly _end = TimeOnly.Parse(schedule.To);

            TimeOnly _time = _start;
            do
            {
                timeSlots.Add(_time.ToString("HH:mm"));
                _time = _time.AddMinutes(data.Slot);
            } while (_time <= _end);
        }
        
        return new DoctorScheduleSlotDto
        {
            Id = doctor.Id,
            Name = doctor.Name,
            Slots = timeSlots
        };
    }

    public async Task<(
        IEnumerable<DoctorDto>, 
        IEnumerable<ScheduleDto>,
        IEnumerable<PatientDto>
    )> Seeds()
    {
        var doctors = await _doctor.CreateBatch();
        var schedules = await _schedule.CreateBatch();
        var patients = await _patient.CreateBatch();

        return (doctors, schedules, patients);
    }

    protected string GetDayName(int id)
    {
        return id switch
        {
            1 => "SU",
            2 => "MO",
            3 => "TU",
            4 => "WE",
            5 => "TH",
            6 => "FR",
            7 => "SA",
        };
    }
}