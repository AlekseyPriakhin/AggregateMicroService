using System.ComponentModel.DataAnnotations.Schema;

using AggregateAndMicroService.Common;

namespace AggregateAndMicroService.Domain.Course;

public class Stage : Aggregate<StageId>
{
    public static readonly int MIN_COMPLETE_PROGRESS = 90;
    private static readonly HashSet<StageType> INSTANT_COMPLETABLE = [
        StageType.Of(StageTypes.Document),
        StageType.Of(StageTypes.Presentation)
    ];

    public static readonly HashSet<StageType> REQUIRED_STAGES = [
        StageType.Of(StageTypes.Test),
    ];

    public string Title { get; private set; }

    public StageType Type { get; private set; }

    public StageDuration Duration { get; private set; }

    public CourseId CourseId { get; private set; }

    public Guid? Previous { get; private set; }

    [NotMapped]
    public bool IsInstantCompletable => INSTANT_COMPLETABLE.Contains(Type);

    // Navigation props 

    public virtual Course Course { get; private set; }

    /* public virtual ICollection<StageCourseCompleting> StageCourseCompletings { get; private set; } */


    public static Stage Create(StageId id, string title, StageType type, StageDuration duration, Guid courseId, Guid? previous = null)
    {
        return new Stage
        {
            Id = id,
            Title = title,
            Type = type,
            Duration = duration,
            CourseId = CourseId.Of(courseId),
            Previous = previous
        };
    }



}
