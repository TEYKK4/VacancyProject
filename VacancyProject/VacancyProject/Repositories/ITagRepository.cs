using VacancyProject.Models;

namespace VacancyProject.Repositories;

public interface ITagRepository : IRepository<Tag>
{
    Task<IEnumerable<int>> GetIdsByJobseeker(int jobseekerId);
    Task<IEnumerable<JobseekerTag>> GetIds();
}