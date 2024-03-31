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

  public static Course Create(CourseId id, CourseStatus status, string title, string description = "")
  {

    if (status.Equals(CourseStatus.Of(Statuses.Archived)))
    {
      throw new CreateWithArchivedStatusException();
    }

    // TODO Переделать ошибку
    /* if(type.Equals(MaterialType.Of(MaterialTypes.Webinar))) {
      throw new DurationRequiredException(type.Value.ToString());
    } */

    var material = new Course
    {
      Id = id,
      Status = status,
      Title = title,
      Description = description,
    };

    return material;
  }


  public CourseCompleting Begin(CourseCompletingId id, UserId userId, CourseId courseId) => CourseCompleting.Create(id, userId, courseId);


  public void UpdateProgress(Stage stage, CourseCompleting courseCompleting, Progress newProgress)
  {

  }

  private void UpdateStatus(CourseStatus status) => Status = status;



  public void UpdateProgress(CourseCompleting participiant, int progress)
  {
    /* if (INSTANT_COMPLETABLE.Any(e => e.Equals(Type)))
    {
      participiant.UpdateProgress(100);
      participiant.Complete();
      return;
    }

    var currentProggres = participiant.Progress.Value;
    var newProgress = progress + currentProggres > 100 ? 100 : progress + currentProggres;

    participiant.UpdateProgress(newProgress);

    if (newProgress > MIN_COMPLETE_PROGRESS && participiant.Status.Equals(ParticipiantStatus.Of(ParticipiantStatuses.InProggess)))
      participiant.Complete(); */
  }

}