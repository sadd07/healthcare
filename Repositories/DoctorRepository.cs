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
}