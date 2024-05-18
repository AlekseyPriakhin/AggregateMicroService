using AggregateAndMicroService.Domain.Course;

using MediatR;

public record CourseStatusChanged : INotification
{
    public Course Course { get; init; }

    public CourseStatusChanged(Course course)
    {
        Course = course;
    }
}
