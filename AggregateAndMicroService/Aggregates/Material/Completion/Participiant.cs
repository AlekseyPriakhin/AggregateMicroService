namespace AggregateAndMicroService.Aggregates.Material;

using AggregateAndMicroService.Common;
using AggregateAndMicroService.Aggregates.User;


public class Participiant : Aggregate<ParticipiantId>
{
  public ParticipiantStatus Status { get; private set; }
  public Progress Progress { get; private set; }

  public virtual Material Material { get; set; }
  public virtual User User { get; set; }

  public string MaterialId { get; set; }

  public string UserId { get; set; }

  public static Participiant Create(ParticipiantId id, ParticipiantStatus status)
  {
    return new()
    {
      Id = id,
      Status = status,
      Progress = Progress.Of(0)
    };
  }

  public void UpdateProgress(int value)
  {
    Progress = Progress.Of(value);
  }

  public void Complete()
  {
    Status = ParticipiantStatus.Of(ParticipiantStatuses.Completed);
  }
}