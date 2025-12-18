using Healthcare.Interfaces.Repositories;
using Healthcare.Models;

namespace Healthcare.Repositories;

public class ScheduleRepository : IScheduleRepository
{
    private readonly AppDbContext _context;

    public ScheduleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ScheduleDto>> CreateBatch()
    {
        var schedules = new List<Schedule>
        {
            new()
            {
                DoctorId = 1,
                DayId = 1,
                From = TimeOnly.Parse("09:00"),
                To = TimeOnly.Parse("12:00"),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new()
            {
                DoctorId = 1,
                DayId = 2,
                From = TimeOnly.Parse("09:00"),
                To = TimeOnly.Parse("12:00"),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new()
            {
                DoctorId = 1,
                DayId = 5,
                From = TimeOnly.Parse("09:00"),
                To = TimeOnly.Parse("12:00"),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new()
            {
                DoctorId = 2,
                DayId = 1,
                From = TimeOnly.Parse("09:00"),
                To = TimeOnly.Parse("12:00"),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new()
            {
                DoctorId = 1,
                DayId = 2,
                From = TimeOnly.Parse("15:00"),
                To = TimeOnly.Parse("17:00"),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        await _context.Schedules.AddRangeAsync(schedules);
        await _context.SaveChangesAsync();
        
        return schedules.Select(s => new ScheduleDto
        {
            Id = s.Id,
            DayId = s.DayId,
            From = s.From.ToString("HH:mm"),
            To = s.To.ToString("HH:mm")
        });
    }
}