using Healthcare.Dto;
using Healthcare.Interfaces.Repositories;
using Healthcare.Models;
using Microsoft.EntityFrameworkCore;

namespace Healthcare.Repositories;

public class DoctorRepository : IDoctorRepository
{
    private readonly AppDbContext _context;

    public DoctorRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DoctorDto>> GetAll()
    {
        return await _context.Doctors.Select(p => new DoctorDto
        {
            Id = p.Id,
            Name = p.Name,
        }).ToListAsync();

    }
    
    public async Task<DoctorDto?> GetById(int id)
    {
        var doctor = await _context.Doctors.FirstOrDefaultAsync(p => p.Id == id);
        if (doctor == null) return null;

        return new DoctorDto
        {
            Id = doctor.Id,
            Name = doctor.Name, 
        };
    }
    
    public async Task<DoctorScheduleDto?> GetSchedulesByDoctorId(int id)
    {
        var doctor = await _context.Doctors
            .Include(d => d.Schedules)
            .FirstOrDefaultAsync(d => d.Id == id);
        if (doctor == null) return null;

        return new DoctorScheduleDto
        {
            Id = doctor.Id,
            Name = doctor.Name,
            Schedules = doctor.Schedules.Select(s => new ScheduleDto
            {
                Id = s.Id,
                DayId = s.DayId,
                From = s.From.ToString("HH:mm"),
                To = s.To.ToString("HH:mm")
            }).ToList()
        };
    }

    public async Task<IEnumerable<DoctorDto>> CreateBatch()
    {
        var doctors = new List<Doctor>
        {
            new()
            {
                Name = "Dr. Alfabet",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new()
            {
                Name = "Dr. Sarah Quinn",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new()
            {
                Name = "Dr. Michael Jordan",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new()
            {
                Name = "Dr. Kevin McCalister",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new()
            {
                Name = "Dr. Medusa",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        await _context.Doctors.AddRangeAsync(doctors);
        await _context.SaveChangesAsync();

        return doctors.Select(d => new DoctorDto
        {
            Id = d.Id,
            Name = d.Name
        });
    }
}