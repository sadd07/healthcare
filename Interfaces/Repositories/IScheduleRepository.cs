using Healthcare.Models;

namespace Healthcare.Interfaces.Repositories;

public interface IScheduleRepository
{
    Task<IEnumerable<ScheduleDto>> CreateBatch();
}