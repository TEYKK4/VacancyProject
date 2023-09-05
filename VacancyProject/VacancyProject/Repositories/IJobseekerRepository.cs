using VacancyProject.Models;

namespace VacancyProject.Repositories;

public interface IJobseekerRepository : IRepository<Jobseeker>
{
    Task<IEnumerable<Jobseeker>> GetMatched(JobTitle jobTitle, Tag tag);
}