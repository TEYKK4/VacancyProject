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

    public Task<IEnumerable<Jobseeker>> GetMatched(JobTitle jobTitle, Tag tag)
    {
        throw new NotImplementedException();
    }
}