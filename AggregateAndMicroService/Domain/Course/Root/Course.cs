using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

using AggregateAndMicroService.Aggregates.Course;
using AggregateAndMicroService.Aggregates.User;
using AggregateAndMicroService.Common;

using Microsoft.Net.Http.Headers;

namespace AggregateAndMicroService.Aggregates.Course;


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

    //public virtual ICollection<Stage> Stages { get; private set; }

    private Course() { }

    private Course(CourseId id, CourseStatus status, string title, StageCount stageCount, string? description)
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
        Status = isStagesValid ? status : CourseStatus.Of(Statuses.Draft);
    }

    public StageCourseCompleting? UpdateStageProgress(UpdateStageParams @params)
    {
        if (!IsActive)
            throw new Exception("Course is not active");

        if (@params.CourseCompleting.IsCompleted)
            throw new Exception("Course already completed");

        var id = @params.StageId;

        var stage = @params.Stages.FirstOrDefault(e => e.Id.Equals(id))
          ?? throw new Exception("Stage not found");

        var completingToUpdate = @params.StagesCompleting.FirstOrDefault(e => e.StageId.Equals(id));

        var stageValidation = ValidateStageProgress(id, @params.Stages, @params.StagesCompleting);

        if (stageValidation.IsSuccess)
        {
            completingToUpdate ??= StageCourseCompleting.Create(@params.CourseCompleting.Id.Value, stage.Id.Value);
            completingToUpdate.UpdateProgress(stage, @params.StageProgress, @params.CourseCompleting);
        }

        return stageValidation.IsFailure ? throw new Exception(stageValidation.ErrorMessage) : completingToUpdate;
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
public record UpdateStageParams
{
    public StageId StageId { get; init; }
    public CourseCompleting CourseCompleting { get; init; }
    public StageProgress StageProgress { get; init; }
    public IEnumerable<StageCourseCompleting> StagesCompleting { get; init; }
    public IEnumerable<Stage> Stages { get; init; }
}

public record CourseDTO
{
    public CourseId Id { get; } = CourseId.Of(Guid.NewGuid());
    public CourseStatus Status { get; init; } = CourseStatus.Of(Statuses.Draft);
    public string Title { get; init; }
    public string Description { get; init; }

}
