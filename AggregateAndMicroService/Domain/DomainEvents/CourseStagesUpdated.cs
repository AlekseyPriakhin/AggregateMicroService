using AggregateAndMicroService.Domain.Course;

using MediatR;

public record CourseStageUpdated : INotification
{
    public Course Course { get; init; }
    public IEnumerable<Stage> Stages { get; init; }
    public CourseStageUpdated(Course course, IEnumerable<Stage> stages)
    {
        Course = course;
        Stages = stages;
    }
}
