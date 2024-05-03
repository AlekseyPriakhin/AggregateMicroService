using AggregateAndMicroService.Domain.Course;

using MediatR;

public record CourseStatusChangedToDrafted : INotification
{
    public Course Course { get; init; }

    public CourseStatusChangedToDrafted(Course course)
    {
        Course = course;
    }
}
