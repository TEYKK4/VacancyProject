namespace VacancyProject.Models;

public class JobTitle
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<Jobseeker> Jobseekers { get; set; } = null!;
}