using System.ComponentModel.DataAnnotations;

using AggregateAndMicroService.Common;

namespace AggregateAndMicroService.Domain.Course;

public class CourseId : ValueObject
{
    public Guid Value { get; }

    private CourseId() { }
    private CourseId(Guid value)
    {
        Value = value;
    }

    public static CourseId Of(Guid guid)
    {
        if (guid == Guid.Empty)
        {
            throw new ArgumentException("Invalid Id");
        }

        return new(guid);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator Guid(CourseId id) => id.Value;
}

public enum Statuses
{
    Active = 0,
    Draft = 1,
    Archived = 2
}

public class CourseStatus : ValueObject
{

    public Statuses Value { get; }

    private CourseStatus() { Value = Statuses.Draft; }
    private CourseStatus(Statuses value)
    {
        Value = value;
    }
    public static CourseStatus Of(Statuses status) => new(status);

    /* public override bool Equals(object? obj)
    {
        return Value == ((MaterialStatus)obj).Value;
    } */

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

public class Duration : ValueObject
{
    public TimeSpan Value { get; }

    private Duration() { Value = TimeSpan.Zero; }

    private Duration(TimeSpan? value)
    {
        Value = value is null ? TimeSpan.Zero : value.Value;
    }

    public static Duration Of(TimeSpan duration) => new(duration);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

public class StageCount : ValueObject
{
    public int Value { get; private set; }

    private StageCount() { Value = 0; }
    private StageCount(int value) { Value = value; }

    public static StageCount Of(IEnumerable<Stage>? stages)
    {
        return new(stages is null ? 0 : stages.Count());
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
