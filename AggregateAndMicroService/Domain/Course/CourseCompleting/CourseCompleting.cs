using AggregateAndMicroService.Common;
using AggregateAndMicroService.Domain.DomainEvents;

using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AggregateAndMicroService.Domain.Course;

public class CourseCompleting : Aggregate<CourseCompletingId>
{
    public Guid UserId { get; private set; }

    public Guid CourseId { get; private set; }

    public CompleteStatus Status { get; private set; }

    public Progress Progress { get; private set; }

    public StagesCountData StagesCountData { get; private set; }

    [NotMapped]
    [JsonIgnore]
    public bool IsCompleted => Status.Equals(CompleteStatus.Of(CompleteStatuses.Completed));


    // Navigation properties
    /* public virtual User.User User { get; private set; }

    public virtual Course Course { get; private set; }

    public virtual ICollection<StageCourseCompleting> StageCourseCompletings { get; private set; } */

    //Methods

    private CourseCompleting() { }

    public static CourseCompleting Create(CourseCompletingId guid, Guid userId, Guid courseId)
    {

        return new CourseCompleting
        {
            Id = guid,
            UserId = userId,
            CourseId = courseId,
            Status = CompleteStatus.Of(CompleteStatuses.InProgress),
            Progress = Progress.Of(0),

        };

    }

    public void UpdateProgress()
    {
        Progress = Progress.Of(StagesCountData.CompletedStages / StagesCountData.TotalStages * 100);
    }

    public void CountNewStage(StageId stageId)
    {
        StagesCountData = StagesCountData.Of(StagesCountData.TotalStages, StagesCountData.CompletedStages + 1);

        if (StagesCountData.CompletedStages == StagesCountData.TotalStages) Complete();
        else UpdateProgress();

    }

    private void Complete()
    {
        Status = CompleteStatus.Of(CompleteStatuses.Completed);
        AddDomainEvent(new CourseCompletedDomainEvent(this));
    }

}
