using Microsoft.AspNetCore.Mvc;
using VacancyProject.DTO;
using VacancyProject.Models;
using VacancyProject.Repositories;

namespace VacancyProject.Endpoints;

public static class JobseekerEndpoints
{
    public static void MapJobseekerEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("jobseekers/");

        group.MapPost("", CreateJobseeker);
        
        group.MapPost("matched", MatchedJobseeker);
        
        group.MapGet("", GetJobseeker);
        
        group.MapDelete("", DeleteJobseeker);
        
        group.MapPut("", UpdateJobseeker);
    }

    private static async Task<IResult> DeleteJobseeker([FromBody] Jobseeker jobseeker, IJobseekerRepository repository)
    {
        if (await repository.Delete(jobseeker))
        {
            return Results.Ok();
        }

        return Results.BadRequest();
    }

    private static async Task<IResult> GetJobseeker(IJobseekerRepository jobseekerRepository,
        ITagRepository tagRepository)
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
    }

    private static async Task<IResult> CreateJobseeker([FromBody] JobseekerRequest jobseeker, IJobseekerRepository repository)
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
    }

    private static async Task<IResult> MatchedJobseeker([FromBody] JobseekerRequest jobseekerForSearch, IJobseekerRepository jobseekerRepository, ITagRepository tagRepository)
    {
        if (await jobseekerRepository.GetMatched(jobseekerForSearch.JobTitleId, jobseekerForSearch.TagIds) is { } orderedJobseekers)
        {
            // var jobseekerTags = await tagRepository.GetIds();
            //
            // var orderedJobseekers = jobseekers.Where(x => x.JobTitleId == jobseekerForSearch.JobTitleId).OrderBy(x =>
            // {
            //     var index = jobseekerForSearch.TagIds.ToList().IndexOf(jobseekerTags.Where(y => y.JobseekerId == x.Id).
            //         Select(y => y.TagId).FirstOrDefault());
            //     
            //     return index == -1 ? int.MaxValue : index;
            // });
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
    }

    private static async Task<IResult> UpdateJobseeker([FromBody] Jobseeker jobseeker, IJobseekerRepository repository)
    {
        if (await repository.Update(jobseeker))
        {
            return Results.Ok();
        }

        return Results.BadRequest();
    }
}