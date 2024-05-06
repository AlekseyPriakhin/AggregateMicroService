using AggregateAndMicroService.Domain.Course;

namespace AggregateAndMicroService.Application.DTO;


public record OutCourseDto
{
    public string Id { get; init; }

    public string Title { get; init; }

    public string Description { get; init; }

    public Statuses Status { get; init; }

    public int StagesCount { get; init; }


}
