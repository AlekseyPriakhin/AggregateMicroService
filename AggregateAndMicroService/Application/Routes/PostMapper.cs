using AggregateAndMicroService.Application.DTO.Request;
using AggregateAndMicroService.Domain.Course;
using AggregateAndMicroService.Domain.CourseProgress;
using AggregateAndMicroService.Domain.User;
using AggregateAndMicroService.Infrastructure;

using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AggregateAndMicroService.Application.Routes;

public static class PostMapper
{

    public static WebApplication MapPostRoutes(this WebApplication app)
    {
        var tags = new[] { "course" };
        app.MapPost("api/v1/courses/{courseId}/start", async ([FromServices] LearningContext context,
        [FromServices] IMediator mediator,
        [FromRoute] string courseId,
        [FromBody] StartCourseDTO dto) =>
        {
            var id = CourseId.Of(Guid.Parse(courseId));
            var course = await context.Courses.Where(e => e.Id.Equals(id))
                                                .Include(e => e.Stages)
                                                .FirstOrDefaultAsync();
            if (course is null) return Results.NotFound($"Курс с таким Id - {courseId} не существует");

            var maybeStartedCourse = context.CourseCompleting.Where(e => e.CourseId.ToString() == courseId
                                                                             && e.UserId.ToString() == dto.UserId)
                                                                                        .FirstOrDefault();

            if (maybeStartedCourse is not null)
            {
                return Results.Ok(maybeStartedCourse);
            }

            var courseCompleting = CourseCompleting.Create(CourseCompletingId.Of(Guid.NewGuid()), UserId.Of(Guid.Parse(dto.UserId)),
            id, course.StageCount.Value);

            courseCompleting.Start(course);

            foreach (var item in courseCompleting.DomainEvents)
            {
                await mediator.Publish(item);
            }
            courseCompleting.ClearDomainEvents();

            await context.SaveEntitiesAsync();

            return Results.Created();
        }).WithTags(tags);


        return app;
    }

}
