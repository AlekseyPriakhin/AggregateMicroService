using AggregateAndMicroService.Domain.Course;

using MediatR;

public record CourseStatusChangedToArchived : INotification
{
    public Course Course { get; init; }

    public CourseStatusChangedToArchived(Course course)
    {
        Course = course;
    }
}
