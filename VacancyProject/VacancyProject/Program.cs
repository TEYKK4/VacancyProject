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

app.MapDelete("jobseeker/", async ([FromBody] Jobseeker jobseeker, IJobseekerRepository repository) =>
{
    if (await repository.Delete(jobseeker))
    {
        return Results.Ok();
    }

    return Results.BadRequest();
});

app.MapGet("jobseeker/", async (IJobseekerRepository jobseekerRepository, ITagRepository tagRepository) =>
{
    if (await jobseekerRepository.Get() is { } jobseekers)
    {
        var jobseekerRequests = new List<JobseekerRequest>();

        foreach (var jobseeker in jobseekers)
        {
            jobseekerRequests.Add(new JobseekerRequest
            {
                Firstname = jobseeker.Firstname,
                Lastname = jobseeker.Lastname,
                Email = jobseeker.Email,
                JobTitleId = jobseeker.JobTitleId,
                TagIds = await tagRepository.GetIdsByJobseeker(jobseeker.Id)
            });
        }

        return Results.Json(jobseekerRequests);
    }

    return Results.BadRequest();
});

app.MapPost("jobseeker/matched", async ([FromBody] JobseekerRequest jobseekerForSearch, IJobseekerRepository jobseekerRepository, ITagRepository tagRepository) =>
{
    if (await jobseekerRepository.Get() is { } jobseekers)
    {
        var jobseekerTags = await tagRepository.GetIds();
        
        var orderedJobseekers = jobseekers.Where(x => x.JobTitleId == jobseekerForSearch.JobTitleId).OrderBy(x =>
        {
            var index = jobseekerForSearch.TagIds.ToList().IndexOf(jobseekerTags.Where(y => y.JobseekerId == x.Id).
                Select(y => y.TagId).FirstOrDefault());
            
            return index == -1 ? int.MaxValue : index;
        });
        // var orderedJobseekers = jobseekers.Where(x => x.JobTitleId == jobseekerForSearch.JobTitleId).OrderBy(x =>
        // {
        //     var firstElement = x.JobseekerTags.Select(y => y.TagId).FirstOrDefault();
        //     var index = jobseekerForSearch.TagIds.ToList().IndexOf(firstElement);
        //     return index == -1 ? int.MaxValue : index;
        // }).ThenBy(x =>
        // {
        //     var firstElement = x.JobseekerTags.Select(y => y.TagId).FirstOrDefault();
        //     return jobseekerForSearch.TagIds.ToList().IndexOf(firstElement);
        // });
        
        // var firstElement = obj.ListB.FirstOrDefault();
        // return targetList.IndexOf(firstElement);
        
        var jobseekerRequests = new List<JobseekerRequest>();

        foreach (var jobseeker in orderedJobseekers)
        {
            jobseekerRequests.Add(new JobseekerRequest
            {
                Firstname = jobseeker.Firstname,
                Lastname = jobseeker.Lastname,
                Email = jobseeker.Email,
                JobTitleId = jobseeker.JobTitleId,
                TagIds = await tagRepository.GetIdsByJobseeker(jobseeker.Id)
            });
        }
        
        return Results.Json(jobseekerRequests);
    }

    return Results.BadRequest();
});

app.MapPut("jobseeker/", async ([FromBody] Jobseeker jobseeker, IJobseekerRepository repository) =>
{
    if (await repository.Update(jobseeker))
    {
        return Results.Ok();
    }

    return Results.BadRequest();
});

app.MapPost("jobseeker/", async ([FromBody] JobseekerRequest jobseeker, IJobseekerRepository repository) =>
{
    var newJobseeker = new Jobseeker
    {
        Firstname = jobseeker.Firstname,
        Lastname = jobseeker.Lastname,
        Email = jobseeker.Email,
        JobTitleId = jobseeker.JobTitleId
    };

    foreach (var jobseekerTagId in jobseeker.TagIds)
    {
        newJobseeker.JobseekerTags.Add(new JobseekerTag
        {
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