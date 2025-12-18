using Healthcare.Interfaces.Repositories;
using Healthcare.Models;
using Microsoft.EntityFrameworkCore;

namespace Healthcare.Repositories;

public class PatientRepository : IPatientRepository
{
    private readonly AppDbContext _context;

    public PatientRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<PatientDto>> GetAll()
    {
        return await _context.Patients.Select(p => new PatientDto
        {
            Id = p.Id,
            Name = p.Name,
        }).ToListAsync();
    }
    
    public async Task<PatientDto> GetById(int id)
    {
        var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Id == id);
        if (patient == null) return null;

        return new PatientDto
        {
            Id = patient.Id,
            Name = patient.Name, 
        };
    }

    public async Task<IEnumerable<PatientDto>> CreateBatch()
    {
        var patients = new List<Patient>
        {
            new()
            {
                Name = "Adam",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new()
            {
                Name = "Bima",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new()
            {
                Name = "Caca",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new()
            {
                Name = "Doni",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new()
            {
                Name = "Edric",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        await _context.Patients.AddRangeAsync(patients);
        await _context.SaveChangesAsync();

        return patients.Select(d => new PatientDto
        {
            Id = d.Id,
            Name = d.Name
        });
    }
}