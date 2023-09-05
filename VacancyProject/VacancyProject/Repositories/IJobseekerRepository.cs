using VacancyProject.Models;

namespace VacancyProject.Repositories;

public interface IJobseekerRepository : IRepository<Jobseeker>
{
    Task<IEnumerable<Jobseeker>> GetMatched(int jobTitleId, IEnumerable<int> tagIds);
}