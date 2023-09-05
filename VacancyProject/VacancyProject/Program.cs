using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VacancyProject.Databases;
using VacancyProject.DTO;
using VacancyProject.Models;
using VacancyProject.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();
builder.Services.AddCors();

builder.Services.AddScoped<IJobseekerRepository, JobseekerRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<IJobTitleRepository, JobTitleRepository>();
builder.Services.AddDbContext<VacancyDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("VacancyDb"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors(x =>
{
    x.AllowAnyHeader();
    x.AllowAnyMethod();
    x.AllowAnyOrigin();
});

app.MapDelete("jobseeker/", async ([FromBody]Jobseeker jobseeker, IJobseekerRepository repository) =>
{
    if (await repository.Delete(jobseeker))
    {
        return Results.Ok();
    }
    return Results.BadRequest();
});

app.MapGet("jobseeker/", async (IJobseekerRepository repository) =>
{
    if (await repository.Get() is { } users)
    {
        return Results.Json(users);
    }
    return Results.BadRequest();
});

app.MapGet("jobseeker/matched", async ([FromBody]Jobseeker jobseeker, IJobseekerRepository repository) =>
{
    if (await repository.Get() is { } jobseekers)
    {
        return Results.Json(jobseekers);
    }
    return Results.BadRequest();
});

app.MapPut("jobseeker/", async ([FromBody]Jobseeker jobseeker, IJobseekerRepository repository) =>
{
    if (await repository.Update(jobseeker))
    {
        return Results.Ok();
    }
    return Results.BadRequest();
});

app.MapPost("jobseeker/", async ([FromBody]JobseekerRequest jobseeker, IJobseekerRepository repository) =>
{
    var newJobseeker = new Jobseeker
    {
        Firstname = jobseeker.Firstname,
        Lastname = jobseeker.Lastname,
        Age = jobseeker.Age,
        Email = jobseeker.Email,
        JobTitleId = jobseeker.JobTitleId
    };

    foreach (var jobseekerTagId in jobseeker.TagIds)
    {
        newJobseeker.JobseekerTags.Add(new JobseekerTag {
            TagId = jobseekerTagId,
            Jobseeker = newJobseeker
        });
    }
    
    if (await repository.Post(newJobseeker))
    {
        return Results.Ok();
    }
    return Results.BadRequest();
});

app.MapGet("job-title/", async (IJobTitleRepository repository) =>
{
    if (await repository.Get() is { } jobTitles)
    {
        return Results.Json(jobTitles);
    }
    return Results.BadRequest();
});

app.MapGet("tag/", async (ITagRepository repository) =>
{
    if (await repository.Get() is { } tags)
    {
        return Results.Json(tags);
    }
    return Results.BadRequest();
});

app.Run();