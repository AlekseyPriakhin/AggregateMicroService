using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

using AggregateAndMicroService.Common;

namespace AggregateAndMicroService.Domain.Course;


[ComplexType]
public class Course : Aggregate<CourseId>
{
    public StageCount StageCount { get; private set; }
    public CourseStatus Status { get; private set; }
    public string Title { get; private set; }
    public string? Description { get; private set; }

    [NotMapped]
    [JsonIgnore]
    public bool IsActive => Status.Equals(CourseStatus.Of(Statuses.Active));

    // navigation properties
    //public virtual ICollection<CourseCompleting> Completings { get; private set; }

    public virtual ICollection<Stage> Stages { get; private set; }

    private Course() : base() { }

    private Course(CourseId id, CourseStatus status, string title, StageCount stageCount, string? description) : base()
    {
        Id = id;
        Status = status;
        Title = title;
        StageCount = stageCount;
        Description = description;
    }

    public static Course Create(CourseDTO courseDto, IEnumerable<Stage> stages)
    {
        if (courseDto.Status.Equals(CourseStatus.Of(Statuses.Archived)))
        {
            throw new CreateWithArchivedStatusException();
        }

        var status = ValidateStages(stages) ? courseDto.Status : CourseStatus.Of(Statuses.Draft);
        return new Course(courseDto.Id, status, courseDto.Title, StageCount.Of(stages), courseDto.Description);

    }

    public void UpdateStatus(CourseStatus status, IEnumerable<Stage> stages)
    {
        var isStagesValid = ValidateStages(stages);
        if (status.Equals(CourseStatus.Of(Statuses.Active))) throw new Exception("Cannot change course status to active due to stage validation");
        else Status = status;
        AddDomainEvent(new CourseStatusChangedToArchived(this));
    }

    private static bool ValidateStages(IEnumerable<Stage> stages)
    {
        if (stages is null || stages.Any() == false)
        {
            return false;
        }

        var stack = new Stack<Stage>(stages);

        foreach (var stage in stages)
        {
            if (Stage.REQUIRED_STAGES.Contains(stage.Type)) stack.Clear();
            else stack.Push(stage);
        }
        return stack.Count == 0;
    }

}

public record CourseDTO
{
    public CourseId Id { get; init; }
    public CourseStatus Status { get; init; } = CourseStatus.Of(Statuses.Draft);
    public string Title { get; init; }
    public string Description { get; init; }

    private CourseDTO() { }

    public CourseDTO(string title, string? description, Guid? id)
    {
        Title = title;
        Description = description ?? string.Empty;
        Id = id is null ? CourseId.Of(Guid.NewGuid()) : CourseId.Of(id.Value);
        Status = CourseStatus.Of(Statuses.Draft);
    }

}
