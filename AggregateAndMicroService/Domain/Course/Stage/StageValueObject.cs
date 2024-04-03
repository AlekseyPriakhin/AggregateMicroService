using AggregateAndMicroService.Common;

namespace AggregateAndMicroService.Aggregates.Course;

public class StageId
{
    public Guid Value { get; private set; }

    private StageId(Guid id)
    {
        Value = id;
    }

    public static StageId Of(Guid guid)
    {
        if (guid == Guid.Empty)
        {
            throw new ArgumentException("Invalid Id");
        }
        return new(guid);
    }

}

public enum StageTypes
{
    Document = 0,
    Webinar = 1,
    Test = 2,
    Presentation = 3
}

public class StageType : ValueObject
{

    public StageTypes Value { get; }

    private StageType(StageTypes value)
    {
        Value = value;
    }

    public static StageType Of(StageTypes materials) => new(materials);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

}

public class StageDuration : ValueObject
{
    public TimeSpan Value { get; }

    public StageDuration() { Value = TimeSpan.Zero; }

    private StageDuration(TimeSpan? value)
    {
        Value = value is null ? TimeSpan.Zero : value.Value;
    }

    public static StageDuration Of(TimeSpan duration) => new(duration);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
