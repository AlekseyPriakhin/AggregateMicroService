using AggregateAndMicroService.Common;

namespace AggregateAndMicroService.Aggregates.Course;

public class CourseId
{

  public Guid Value { get; }

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

  public Duration() { Value = TimeSpan.Zero; }

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
