using AggregateAndMicroService.Domain.Course;

namespace AggregateAndMicroService.Domain.DTO;

public record StageDTO
{
    public StageId StageId { get; init; }
    public string Title { get; init; }
    public StageType Type { get; init; }
    public StageDuration Duration { get; init; }
    public Guid CourseId { get; init; }
    public Guid? Previous { get; init; }

    public StageDTO(Stage stage)
    {
        StageId = stage.Id;
        Title = stage.Title;
        Type = stage.Type;
        Duration = stage.Duration;
        CourseId = stage.CourseId;
        Previous = stage.Previous;
    }

}
