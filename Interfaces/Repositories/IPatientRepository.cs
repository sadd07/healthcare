using Healthcare.Models;

namespace Healthcare.Interfaces.Repositories;

public interface IPatientRepository
{
    Task<IEnumerable<PatientDto>> GetAll();
    Task<PatientDto> GetById(int id);
    Task<IEnumerable<PatientDto>> CreateBatch();
}