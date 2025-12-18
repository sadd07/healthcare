using Healthcare.Dto;
using Healthcare.Interfaces.Repositories;
using Healthcare.Models;

namespace Healthcare.Repositories;

public class DoctorRepository : IDoctorRepository
{
    private readonly List<Doctor> models = new();

    public async Task<IEnumerable<DoctorDto>> GetAll()
    {
        return models.Select(p => new DoctorDto
        {
            Id = p.Id,
            Name = p.Name,
        }).ToList();

    }
    
    public async Task<DoctorDto?> GetById(int id)
    {
        var doctor = models.FirstOrDefault(p => p.Id == id);
        if (doctor == null) return null;

        return new DoctorDto
        {
            Id = doctor.Id,
            Name = doctor.Name, 
        };
    }

    public async Task<IEnumerable<DoctorDto>> CreateBatch()
    {
        var doctors = new List<Doctor>
        {
            new()
            {
                Id = 1, 
                Name = "Dr. Alfabet",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = 2,
                Name = "Dr. Sarah Quinn",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = 3,
                Name = "Dr. Michael Jordan",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = 4,
                Name = "Dr. Kevin McCalister",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = 5,
                Name = "Dr. Medusa",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        models.AddRange(doctors);
        
        return doctors.Select(d => new DoctorDto
        {
            Id = d.Id,
            Name = d.Name
        });
    }
}