namespace VacancyProject.Models;

public class JobseekerTag
{
    public Jobseeker Jobseeker { get; set; } = null!;
    public int JobseekerId { get; set; }
    
    public Tag Tag { get; set; } = null!;
    public int TagId { get; set; }
}