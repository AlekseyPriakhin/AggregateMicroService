using AggregateAndMicroService.Domain.Course;
using AggregateAndMicroService.Infrastructure;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using static AggregateAndMicroService.Application.Mappers.CourseMapper;
using static AggregateAndMicroService.Application.DTO.Response.ResponseBuilder;
using static AggregateAndMicroService.Application.DTO.Response.PaginationService;

namespace AggregateAndMicroService.Application.Routes;

public static class GetMapper
{
    public static WebApplication MapGetRoutes(this WebApplication app)
    {
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
        .WithOpenApi();

        return app;

    }
}
