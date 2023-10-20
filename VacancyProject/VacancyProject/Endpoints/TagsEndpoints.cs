using VacancyProject.Repositories;

namespace VacancyProject.Endpoints;

public static class TagsEndpoints
{
    public static void MapTagsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("tags/", GetTag);
    }

    private static async Task<IResult> GetTag(ITagRepository repository)
    {
        if (await repository.Get() is { } tags)
        {
            return Results.Json(tags);
        }

        return Results.BadRequest();
    }
}