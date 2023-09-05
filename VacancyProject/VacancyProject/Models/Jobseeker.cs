namespace VacancyProject.Models;

public class Jobseeker
{
    public int Id { get; set; }
    public string Firstname { get; set; } = null!;
    public string Lastname { get; set; } = null!;
    public string Email { get; set; } = null!;
    public JobTitle JobTitle { get; set; } = null!;
    public int JobTitleId { get; set; }
    public ICollection<JobseekerTag> JobseekerTags { get; set; } = new List<JobseekerTag>();
}

