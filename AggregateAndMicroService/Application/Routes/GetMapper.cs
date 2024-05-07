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
                                    .Select(e => ToResponseCourseDto(e))
                                    .AsNoTracking()
                                    .ToListAsync(token);

            var response = CreateResponse(items, GetPagination(items, page, perPage, total));
            return Results.Ok(response);
        }).WithOpenApi();

        app.MapGet("api/v1/courses/{courseId}", async ([FromServices] LearningContext context, [FromRoute] string courseId, CancellationToken token) =>
        {
            var course = await context.Courses.FindAsync(CourseId.Of(Guid.Parse(courseId)), token);
            return course is null
                ? Results.NotFound(CreateResponse($"Курс с таким Id - {courseId} не существует"))
                : Results.Ok(CreateResponse(ToResponseCourseDto(course)));
        }).WithOpenApi();

        return app;

    }
}
