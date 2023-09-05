using Microsoft.EntityFrameworkCore;
using VacancyProject.Models;

namespace VacancyProject.Databases;

public class VacancyDbContext : DbContext
{
    public VacancyDbContext(DbContextOptions<VacancyDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Jobseekers to Tags
        modelBuilder.Entity<JobseekerTag>()
            .HasKey(x => new { x.JobseekerId, x.TagId });
        
        modelBuilder.Entity<JobseekerTag>()
            .HasOne(jt => jt.Jobseeker)
            .WithMany(j => j.JobseekerTags)
            .HasForeignKey(jt => jt.JobseekerId);

        modelBuilder.Entity<JobseekerTag>()
            .HasOne(jt => jt.Tag)
            .WithMany(t => t.JobseekerTags)
            .HasForeignKey(jt => jt.TagId);

        
        // JobTitle to Jobseekers
        modelBuilder.Entity<Jobseeker>()
            .HasOne(j => j.JobTitle)
            .WithMany(jt => jt.Jobseekers)
            .HasForeignKey(j => j.JobTitleId);
        
        
        modelBuilder.Entity<JobTitle>().HasData(
            new JobTitle { Id = 1, Name = "Backend Developer" },
            new JobTitle { Id = 2, Name = "Frontend Developer" },
            new JobTitle { Id = 3, Name = "Fullstack Developer" }
        );
        
        modelBuilder.Entity<Tag>().HasData(
            new Tag { Id = 1, Name = ".NET" },
            new Tag { Id = 2, Name = "CSS" },
            new Tag { Id = 3, Name = "HTML" },
            new Tag { Id = 4, Name = "Next.js" },
            new Tag { Id = 5, Name = "React.js" },
            new Tag { Id = 6, Name = "Typescript" },
            new Tag { Id = 7, Name = "C#" },
            new Tag { Id = 8, Name = "ASP.NET" },
            new Tag { Id = 9, Name = "HTML" },
            new Tag { Id = 10, Name = "Javascript" }
        );

        base.OnModelCreating(modelBuilder);
    }
    
    public DbSet<Jobseeker> Jobseekers { get; set; } = null!; 
    public DbSet<JobTitle> JobTitles { get; set; } = null!; 
    public DbSet<Tag> Tags { get; set; } = null!;
    public DbSet<JobseekerTag> JobseekerTags { get; set; } = null!;
}