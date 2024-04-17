using AggregateAndMicroService.Common;

namespace AggregateAndMicroService.Domain.Course;

public class StageCourseCompleting
{

    #region Props

    public Guid CourseCompletingId { get; private set; }

    public Guid StageId { get; private set; }

    public StageProgress StageProgress { get; private set; }

    #endregion

    #region Navigation Props

    /* public virtual CourseCompleting CourseCompleting { get; private set; }
    public virtual Stage Stage { get; private set; } */

    #endregion

    #region Methods

    private StageCourseCompleting() { }
    private StageCourseCompleting(Guid completingId, Guid stageId)
    {

        CourseCompletingId = completingId;
        StageId = stageId;
        StageProgress = StageProgress.Of(0);
    }


    public static StageCourseCompleting Create(Guid completingId, Guid stageId)
    {
        return new StageCourseCompleting(completingId, stageId);
    }

    public void UpdateProgress(Stage stage, StageProgress newProgress, CourseCompleting courseCompleting)
    {
        if (StageProgress.Value > Stage.MIN_COMPLETE_PROGRESS) throw new Exception("Already Completed");

        if (stage.IsInstantCompletable)
        {
            StageProgress = StageProgress.Of(100);
            courseCompleting.CountNewStage(stage.Id);
            return;
        }

        StageProgress = newProgress;

        if (StageProgress.Value > Stage.MIN_COMPLETE_PROGRESS) courseCompleting.CountNewStage(stage.Id);
    }


    #endregion

}

// Value objects

public class StageProgress : ValueObject
{
    public int Value { get; }

    private StageProgress() { Value = 0; }

    private StageProgress(int value = 0)
    {
        Value = value;
    }

    public static StageProgress Of(int value)
    {
        if (value < 0 || value > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "Value must be between 0 and 100");
        }

        return new(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
