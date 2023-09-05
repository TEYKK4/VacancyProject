namespace VacancyProject.DTO;

public class JobseekerRequest
{
    public string Firstname { get; set; } = null!;
    public string Lastname { get; set; } = null!;
    public string Email { get; set; } = null!;
    public int JobTitleId { get; set; }
    public IEnumerable<int> TagIds { get; set; } = null!;
}