using AggregateAndMicroService.Common;
using AggregateAndMicroService.Aggregates.User;


namespace AggregateAndMicroService.Aggregates.Course;


public class CourseCompleting : Aggregate<CourseCompletingId>
{
    public UserId UserId { get; private set; }

    public CourseId CourseId { get; private set; }

    public CompleteStatus Status { get; private set; }

    public Progress Progress { get; private set; }


    // Navigation properties
    public virtual User.User User { get; private set; }

    public virtual Course Course { get; private set; }

    //Methods

    public static CourseCompleting Create(CourseCompletingId guid, UserId userId, CourseId courseId)
    {

        return new CourseCompleting
        {
            Id = guid,
            UserId = userId,
            CourseId = courseId,
            Status = CompleteStatus.Of(CompleteStatutes.InProggess),
            Progress = Progress.Of(0),

        };

    }

    public void UpdateProgress(StageId stageId, Progress newProgress)
    {

    }

    private void UpdateStatus(StageId stageId)
    {


    }

}