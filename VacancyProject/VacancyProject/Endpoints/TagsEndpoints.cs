using Carter;
using VacancyProject.Repositories;

namespace VacancyProject.Endpoints;

public class TagsEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("tags/", GetTag);
    }
    
    // public static void MapTagsEndpoints(this IEndpointRouteBuilder app)
    // {
    //     app.MapGet("tags/", GetTag);
    // }

    private static async Task<IResult> GetTag(ITagRepository repository)
    {
        if (await repository.Get() is { } tags)
        {
            return Results.Json(tags);
        }

        return Results.BadRequest();
    }
}