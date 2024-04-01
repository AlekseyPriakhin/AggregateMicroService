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

  // navigation properties
  public virtual ICollection<CourseCompleting> Completings { get; private set; }

  public virtual ICollection<Stage> Stages { get; private set; }

  public static Course Create(CourseId id, CourseStatus status, string title, string description = "")
  {

    if (status.Equals(CourseStatus.Of(Statuses.Archived)))
    {
      throw new CreateWithArchivedStatusException();
    }


    var material = new Course
    {
      Id = id,
      Status = status,
      Title = title,
      Description = description,
    };

    return material;
  }

  public void UpdateStatus(CourseStatus status) => Status = status;

  public void UpdateStageProgress(UpdateStageParams updateStageParams) 
  {
    if(Status.Equals(CourseStatus.Of(Statuses.Draft)) || Status.Equals(CourseStatus.Of(Statuses.Archived)))
      throw new Exception("Course is not active");
    
    if(updateStageParams.CourseCompleting.Status.Equals(CompleteStatus.Of(CompleteStatutes.Completed))) 
      throw new Exception("Course already completed");

    updateStageParams.StageCourseCompleting.UpdateProgress(
      updateStageParams.Stage,
      updateStageParams.StageProgress,
      updateStageParams.CourseCompleting);
  }

}
  public record UpdateStageParams {
    public Stage Stage { get; init; }
    public CourseCompleting CourseCompleting { get; init; }

    public StageCourseCompleting StageCourseCompleting { get; init; }
    public StageProgress StageProgress { get; init; }
  } 
