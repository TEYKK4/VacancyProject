namespace VacancyProject.Models;

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<JobseekerTag> JobseekerTags { get; set; } = null!;
}