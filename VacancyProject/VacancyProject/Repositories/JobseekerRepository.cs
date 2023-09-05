using Microsoft.EntityFrameworkCore;
using VacancyProject.Databases;
using VacancyProject.Models;

namespace VacancyProject.Repositories;

public class JobseekerRepository : IJobseekerRepository
{
    private readonly VacancyDbContext _context;

    public JobseekerRepository(VacancyDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Jobseeker>> Get()
    {
        return await _context.Jobseekers.ToArrayAsync();
    }

    public async Task<bool> Post(Jobseeker item)
    {
        await _context.AddAsync(item);

        return await _context.SaveChangesAsync() >= 1;
    }

    public async Task<bool> Update(Jobseeker item)
    {
        _context.Update(item);
        
        return await _context.SaveChangesAsync() >= 1;
    }

    public async Task<bool> Delete(Jobseeker item)
    {
        _context.Remove(item);
        
        return await _context.SaveChangesAsync() >= 1;
    }

    public async Task<IEnumerable<Jobseeker>> GetMatched(int jobTitleId, IEnumerable<int> tagIds)
    {
        var jobseekers = await Get();
        
        var jobseekerTags = await _context.JobseekerTags.ToArrayAsync();
        
        return jobseekers.Where(x => x.JobTitleId == jobTitleId).OrderBy(x =>
        {
            var index = tagIds.ToList().IndexOf(jobseekerTags.Where(y => y.JobseekerId == x.Id).
                Select(y => y.TagId).FirstOrDefault());
            
            return index == -1 ? int.MaxValue : index;
        });
    }
}