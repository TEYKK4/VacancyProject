using Microsoft.EntityFrameworkCore;
using VacancyProject.Databases;
using VacancyProject.Models;

namespace VacancyProject.Repositories;

public class TagRepository : ITagRepository
{
    private readonly VacancyDbContext _context;

    public TagRepository(VacancyDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Tag>> Get()
    {
        return await _context.Tags.ToArrayAsync();
    }
    
    public async Task<IEnumerable<int>> GetIdsByJobseeker(int jobseekerId)
    {
        return await _context.JobseekerTags.Where(x => x.JobseekerId == jobseekerId).Select(x => x.TagId).ToArrayAsync();
    }

    public async Task<bool> Post(Tag item)
    {
        await _context.AddAsync(item);

        return await _context.SaveChangesAsync() >= 1;
    }

    public async Task<bool> Update(Tag item)
    {
        _context.Update(item);
        
        return await _context.SaveChangesAsync() >= 1;
    }

    public async Task<bool> Delete(Tag item)
    {
        _context.Remove(item);
        
        return await _context.SaveChangesAsync() >= 1;
    }
}