using AggregateAndMicroService.Common;
using AggregateAndMicroService.Domain.Course;
using AggregateAndMicroService.Domain.DomainEvents;

using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AggregateAndMicroService.Domain.CourseProgress;

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

    public StageCourseCompleting? UpdateStageProgress(UpdateStageParams @params)
    {
        if (!@params.Course.IsActive) throw new Exception("Course is not active");

        if (IsCompleted) throw new Exception("Course already completed");

        var stageId = @params.StageId;
        var stage = @params.Stages.FirstOrDefault(e => e.Id.Equals(stageId)) ?? throw new Exception("Stage not found");

        var completingToUpdate = @params.StagesCompleting.FirstOrDefault(e => e.StageId.Equals(stageId));

        var stageValidation = ValidateStageProgress(stageId, @params.Stages, @params.StagesCompleting);

        if (stageValidation.IsSuccess)
        {
            if (completingToUpdate is null)
            {
                completingToUpdate ??= StageCourseCompleting.Create(Id.Value, stage.Id.Value);
                AddDomainEvent(new StageCompletingCreated(completingToUpdate));
            }
            completingToUpdate.UpdateProgress(stage, @params.StageProgress, this);
        }

        return stageValidation.IsFailure ? throw new Exception(stageValidation.ErrorMessage) : completingToUpdate;
    }

    public void Start(Course.Course course)
    {

        if (IsCompleted) throw new Exception("Course already completed");

        var firstStage = course.Stages.FirstOrDefault();
        if (firstStage is null) throw new Exception("Course has no stages");

        var stageCompleting = StageCourseCompleting.Create(Id.Value, firstStage.Id.Value);

        AddDomainEvent(new CourseStartedDomainEvent(this, stageCompleting));
    }

    public void CountNewStage(StageId stageId)
    {
        StagesCountData = StagesCountData.Of(StagesCountData.TotalStages, StagesCountData.CompletedStages + 1);

        if (StagesCountData.CompletedStages == StagesCountData.TotalStages) Complete();
        else UpdateProgress();

    }

    public void TryUpdateCompleting(UpdateParams @params)
    {
        if (Status.Equals(CompleteStatus.Of(CompleteStatuses.Completed))) return;

        // Todo
    }

    private void UpdateProgress()
    {
        Progress = Progress.Of(StagesCountData.CompletedStages / StagesCountData.TotalStages * 100);
    }


    private void Complete()
    {
        Status = CompleteStatus.Of(CompleteStatuses.Completed);
        AddDomainEvent(new CourseCompletedDomainEvent(this));
    }

    private static Result<bool> ValidateStageProgress(StageId stageId, IEnumerable<Stage> stages, IEnumerable<StageCourseCompleting> stagesCompleting)
    {

        if (stagesCompleting is null || stagesCompleting.Any() == false)
        {
            return Result<bool>.Failure("Stages empty");
        }

        var stageToComplete = stages.FirstOrDefault(x => x.Id.Equals(stageId)) ?? throw new Exception($"Stage {stageId} not found");

        if (stageToComplete.Previous is null) return Result<bool>.Success(true);


        var prevStage = stagesCompleting.FirstOrDefault(x => x.StageId.Equals(stageToComplete.Previous))
                  ?? throw new Exception($"Stage {stageToComplete.Previous} before stage {stageId} not found");

        var isPrevCompleted = stagesCompleting.FirstOrDefault(e => e.StageId.Equals(prevStage.StageId)
                                                && e.StageProgress.Value >= Stage.MIN_COMPLETE_PROGRESS) is not null;

        return isPrevCompleted ? Result<bool>.Success(isPrevCompleted)
                               : Result<bool>.Failure($"Previous stage {prevStage.StageId} not completed");
    }

}


public record UpdateParams
{
    public Course.Course Course { get; init; }
    public IEnumerable<StageCourseCompleting> StagesCompleting { get; init; }
    public IEnumerable<Stage> Stages { get; init; }
}

public record UpdateStageParams : UpdateParams
{
    public StageId StageId { get; init; }
    public StageProgress StageProgress { get; init; }
}


public record StartCourseParams
{
    public Course.Course Course { get; init; }

}
