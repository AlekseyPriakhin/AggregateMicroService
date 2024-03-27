namespace AggregateAndMicroService.Aggregates.Material;

using AggregateAndMicroService.Common;
using AggregateAndMicroService.Aggregates.User;


public class Participiant
{
  public UserId UserId { get; private set; }
  public MaterialId MaterialId { get; private set; }
  public ParticipiantStatus Status { get; private set; }
  public Progress Progress { get; private set; }


  //Navigation properties
  public virtual Material Material { get; private set; }
  public virtual User User { get; private set; }



  public static Participiant Create(MaterialId materialId, UserId userId, ParticipiantStatus status)
  {
    return new()
    {
      MaterialId = materialId,
      UserId = userId,
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