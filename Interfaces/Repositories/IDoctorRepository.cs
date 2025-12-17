using Healthcare.Dto;

namespace Healthcare.Interfaces.Repositories;

public interface IDoctorRepository
{
    Task<IEnumerable<DoctorDto>> GetAll();
    Task<DoctorDto?> GetById(int id);
}