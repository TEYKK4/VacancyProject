using VacancyProject.Repositories;

namespace VacancyProject.Endpoints;

public static class JobTitleEndpoints
{
    public static void MapJobTitleEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("job-titles/", GetJobTitle);
    }

    private static async Task<IResult> GetJobTitle(IJobTitleRepository repository)
    {
        if (await repository.Get() is { } jobTitles)
        {
            return Results.Json(jobTitles);
        }

        return Results.BadRequest();
    }
}