using Carter;
using VacancyProject.Repositories;

namespace VacancyProject.Endpoints;

public class JobTitleEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("job-titles/", GetJobTitle);
    }
    
    // public static void MapJobTitleEndpoints(this IEndpointRouteBuilder app)
    // {
    //     app.MapGet("job-titles/", GetJobTitle);
    // }

    private static async Task<IResult> GetJobTitle(IJobTitleRepository repository)
    {
        if (await repository.Get() is { } jobTitles)
        {
            return Results.Json(jobTitles);
        }

        return Results.BadRequest();
    }
}