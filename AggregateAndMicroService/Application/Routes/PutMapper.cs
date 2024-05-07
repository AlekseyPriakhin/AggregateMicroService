using AggregateAndMicroService.Application.DTO.Request;
using AggregateAndMicroService.Domain.Course;
using AggregateAndMicroService.Infrastructure;

using Microsoft.AspNetCore.Mvc;

using static AggregateAndMicroService.Application.DTO.Response.ResponseBuilder;

using Microsoft.EntityFrameworkCore;
using AggregateAndMicroService.Application.Mappers;

namespace AggregateAndMicroService.Application.Routes;

public static class PutMapper
{
    public static WebApplication MapPutRoutes(this WebApplication app)
    {
        app.MapPut("api/v1/courses/{courseId}/status", async ([FromServices] LearningContext context,
                    [FromRoute] string courseId,
                    [FromBody] UpdateCourseStatusDTO dto,
                    CancellationToken token) =>
        {

            var course = await context.Courses
                                .Where(e => e.Id.Equals(CourseId.Of(Guid.Parse(courseId))))
                                .Include(e => e.Stages)
                                .FirstOrDefaultAsync(token);
            if (course is null) return Results.NotFound(CreateResponse($"Курс с таким Id - {courseId} не существует"));

            var stages = course.Stages;

            course.UpdateStatus(CourseStatus.Of(dto.Status), stages);

            await context.SaveEntitiesAsync();

            return Results.Ok(CreateResponse(CourseMapper.ToCourseResponseDto(course)));

        });
        return app;
    }
}
