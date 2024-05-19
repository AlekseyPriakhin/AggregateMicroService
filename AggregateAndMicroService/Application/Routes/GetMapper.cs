using AggregateAndMicroService.Domain.Course;
using AggregateAndMicroService.Infrastructure;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using static AggregateAndMicroService.Application.Mappers.CourseMapper;
using static AggregateAndMicroService.Application.DTO.Response.ResponseBuilder;
using static AggregateAndMicroService.Application.DTO.Response.PaginationService;
using AggregateAndMicroService.Application.Mappers;
using AggregateAndMicroService.Infrastructure.Kafka;
using System.Text.Json;

namespace AggregateAndMicroService.Application.Routes;

public class TestMessage
{
    public string Value { get; set; }
}

public static class GetMapper
{
    public static WebApplication MapGetRoutes(this WebApplication app)
    {
        var tags = new[] { "course" };

        app.MapGet("api/v1/courses", async ([FromServices] LearningContext context, CancellationToken token, [FromQuery] int page = 1, [FromQuery] int perPage = 10) =>
        {
            var query = context.Courses;

            var total = await query.CountAsync(token);
            var items = await query.Skip((page - 1) * perPage)
                                    .Take(perPage)
                                    .Include(e => e.Stages)
                                    .Select(e => ToCourseResponseDto(e))
                                    .AsNoTracking()
                                    .ToListAsync(token);

            var response = CreateResponse(items, GetPagination(items, page, perPage, total));
            return Results.Ok(response);
        })
        .WithName("GetCourses")
        .WithTags(tags)
        .WithOpenApi();

        app.MapGet("api/v1/courses/{courseId}", async ([FromServices] LearningContext context, [FromRoute] string courseId, CancellationToken token) =>
        {
            var course = await context.Courses
                                .Where(e => e.Id.Equals(CourseId.Of(Guid.Parse(courseId))))
                                .Include(e => e.Stages)
                                .AsNoTracking()
                                .FirstOrDefaultAsync(token);
            if (course is null) return Results.NotFound(CreateResponse($"Курс с таким Id - {courseId} не существует"));

            return Results.Ok(CreateResponse(ToCourseResponseDto(course)));
        })
        .WithName("GetCourse")
        .WithTags(tags)
        .WithOpenApi();


        app.MapGetUserRoutes();
        return app;

    }

    private static WebApplication MapGetUserRoutes(this WebApplication app)
    {
        app.MapGet("api/v1/users", async ([FromServices] LearningContext context, CancellationToken token, [FromQuery] int page = 1, [FromQuery] int perPage = 10) =>
        {

            var query = context.Users.AsQueryable();
            var users = query.Skip((page - 1) * perPage)
                                    .Take(perPage)
                                    .AsNoTracking()
                                    .Select(e => UserMapper.ToUserResponseDto(e))
                                    .ToList();


            var totalCount = await query.CountAsync(token);
            return Results.Ok(CreateResponse(users, GetPagination(users, page, perPage, totalCount)));
        }).WithTags(["user"]);

        return app;
    }
}
