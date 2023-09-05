using Microsoft.EntityFrameworkCore;
using VacancyProject.Databases;
using VacancyProject.Models;

namespace VacancyProject.Repositories;

public class JobTitleRepository : IJobTitleRepository
{
    private readonly VacancyDbContext _context;

    public JobTitleRepository(VacancyDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<JobTitle>> Get()
    {
        return await _context.JobTitles.ToArrayAsync();
    }

    public async Task<bool> Post(JobTitle item)
    {
        await _context.AddAsync(item);

        return await _context.SaveChangesAsync() >= 1;
    }

    public async Task<bool> Update(JobTitle item)
    {
        _context.Update(item);
        
        return await _context.SaveChangesAsync() >= 1;
    }

    public async Task<bool> Delete(JobTitle item)
    {
        _context.Remove(item);
        
        return await _context.SaveChangesAsync() >= 1;
    }
}