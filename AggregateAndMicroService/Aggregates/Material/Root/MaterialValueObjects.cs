using AggregateAndMicroService.Common;

namespace AggregateAndMicroService.Aggregates.Material;

public class MaterialId
{

  public Guid Value { get; }

  private MaterialId(Guid value)
  {
    Value = value;
  }

  public static MaterialId Of(Guid guid)
  {
    if (guid == Guid.Empty)
    {
      throw new ArgumentException($"Invalid Id - {guid}");
    }

    return new(guid);
  }

  public static implicit operator Guid(MaterialId id) => id.Value;
}

public enum MaterialTypes
{
  Document = 0,
  Webinar = 1,
  Course = 2,
  Presentation = 3
}

public class MaterialType : ValueObject
{

  public MaterialTypes Value { get; }

  private MaterialType(MaterialTypes value)
  {
    Value = value;
  }

  public static MaterialType Of(MaterialTypes materials) {
    return new MaterialType(materials);
  }

  protected override IEnumerable<object> GetEqualityComponents()
  {
    yield return Value;
  }

}

public enum Statuses
{
  Active = 0,
  Draft = 1,
  Archived = 2
}

public class MaterialStatus : ValueObject
{

  public Statuses Value { get; }

  private MaterialStatus(Statuses value)
  {
    Value = value;
  }
  public static MaterialStatus Of(Statuses status) => new(status);

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
